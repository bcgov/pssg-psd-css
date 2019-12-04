import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, Subject, Subscription, forkJoin } from 'rxjs';
import { filter, first } from 'rxjs/operators';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { setPropertyTypes } from '@actions/property-types.actions';
import { Complaint } from '@models/complaint.model';
import { PropertyType } from '@models/property-type.model';
import { Status } from '@models/status.model';
import { ComplaintDataService } from '@services/complaint-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-csa-form',
  templateUrl: './csa-form.component.html',
  styleUrls: ['./csa-form.component.scss']
})
export class CsaFormComponent extends FormBase implements OnInit, OnDestroy {
  public propertyTypes: Observable<PropertyType[]>;
  submittingForm: Subscription;
  statusSubscription: Subscription;
  submissionResult: Subject<boolean>;
  loaded: boolean;
  faCalendar = faCalendar;
  propertyTypeOther = 862570008;
  authorizationToken : string;
  captchaApiBaseUrl : string;

  constructor(
    private formDataService: ComplaintDataService,
    private router: Router,
    private propertyTypesStore: Store<{ properyTypes: PropertyType[] }>,
    private statusStore: Store<{ status: Status }>,
    private formBuilder: FormBuilder
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      complaintDetails: this.formBuilder.group({
        propertyType: [null],
        otherPropertyType: [''],
        name: [''],
        address: this.formBuilder.group({
          unit: [''],
          line1: [''],
          city: ['', Validators.required],
          provinceState: [{ value: 'BC', disabled: true }],
          country: [{ value: 'Canada', disabled: true }],
          zipPostalCode: [''],
        }),
        occupantName: [''],
        ownerName: [''],
        description: ['', Validators.required],
        problems: ['', Validators.required],
      }),
      complainantContactInfo: this.formBuilder.group({
        firstName: ['', Validators.required],
        middleName: [''],
        lastName: ['', Validators.required],
        fax: ['', this.maskedTelephoneValidator],
        phone: [''],
        email: ['', Validators.email],
      }),
      complainantMailingAddress: this.formBuilder.group({
        unit: [''],
        line1: ['', Validators.required],
        city: ['', Validators.required],
        provinceState: ['BC', Validators.required],
        country: ['Canada', Validators.required],
        zipPostalCode: [''],
      }),
      acceptTerms: [''],
    });

    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    const phoneEmailValidator = this.atLeastOneRequired(complainantPhone, complainantEmail);
    complainantPhone.setValidators([ phoneEmailValidator, this.maskedTelephoneValidator ]);
    complainantEmail.valueChanges.subscribe(email => {
      complainantPhone.updateValueAndValidity();
    });

    // fetch property types from back-end and update store
     this.formDataService.getPropertyTypes().subscribe(result => {
      this.propertyTypesStore.dispatch(setPropertyTypes({ propertyTypes: result }));
    });

    // retrieve valid property types from store
    this.propertyTypes = this.propertyTypesStore.pipe(
      select('propertyTypes'),
      filter(propertyTypes => Array.isArray(propertyTypes))
    );

    // retrieve valid status from store
    const statusObservable =  this.statusStore.pipe(
      select(state => state.status),
      filter(status => Boolean(status && status.captchaApiUrl))
    );

    // retrieve captcha api URL from status
    this.statusSubscription = statusObservable.subscribe(status => {
      this.captchaApiBaseUrl = status.captchaApiUrl;
    });

    // set page as loaded once valid property types and status have been retrieved
    forkJoin([
      this.propertyTypes.pipe(first()),
      statusObservable.pipe(first()),
    ]).subscribe(() => {
      this.loaded = true;
    });
  }

  ngOnDestroy() {
    this.statusSubscription.unsubscribe();
  }

  checkTelephoneInvalid() {
    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    return (complainantPhone.touched || complainantEmail.touched) && !complainantPhone.valid;
  }

  save(data: Complaint): Subject<boolean> {
    this.submissionResult = new Subject<boolean>();

    this.submittingForm = this.formDataService.submitCsaForm(data).subscribe({
      error: err => this.submissionResult.error(err)
    });

    return this.submissionResult;
  }

  submit() {
    if (this.form.valid) {
      const formData = this.form.value;
      const data = <Complaint>{
        details: { ...formData.complaintDetails },
        complainant: {
          ...formData.complainantContactInfo,
          address: formData.complainantMailingAddress,
        },
        authorizationToken: this.authorizationToken,
      };

      this.save(data).subscribe({
        error: (err: any) => {
          console.error(err);
          this.router.navigate(['/error']);
        },
        complete: () => {
          this.router.navigate(['/complaint-submitted']);
        }
      });
    } else {
      this.form.markAllAsTouched();
    }
  }

  onBusyStop() {
    this.submissionResult.complete();
  }
}
