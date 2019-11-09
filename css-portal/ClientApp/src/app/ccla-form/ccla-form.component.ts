import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, Subject, Subscription } from 'rxjs';
import { NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { setPropertyTypes } from '@actions/property-types.actions';
import { Complaint } from '@models/complaint.model';
import { PropertyType } from '@models/property-type.model';
import { ComplaintService } from '@services/form-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-ccla-form',
  templateUrl: './ccla-form.component.html',
  providers: [
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
  ]
})
export class CclaFormComponent extends FormBase implements OnInit {
  public propertyTypes: Observable<PropertyType[]>;
  submittingForm: Subscription;
  submissionResult: Subject<boolean>;
  loaded: Boolean;
  faCalendar = faCalendar;

  constructor(
    private formDataService: ComplaintService,
    private router: Router,
    private propertyTypesStore: Store<{ properyTypes: PropertyType[] }>,
    private formBuilder: FormBuilder
  ) {
    super();
  }

  ngOnInit() {
    this.propertyTypes = this.propertyTypesStore.pipe(select('propertyTypes'));

    this.form = this.formBuilder.group({
      complaintDetails: this.formBuilder.group({
        name: [''],
        address: this.formBuilder.group({
          unit: [''],
          line1: [''],
          city: ['', Validators.required],
          provinceState: [{ value: 'BC', disabled: true }],
          country: [{ value: 'Canada', disabled: true }],
          zipPostalCode: [''],
        }),
        problems: ['', Validators.required],
      }),
      anonymousComplainant: [false],
      complainantContactInfo: this.formBuilder.group({
        firstName: ['', Validators.required],
        middleName: [''],
        lastName: ['', Validators.required],
        fax: [''],
        governmentOrganization: [''],
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
    });

    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    const phoneEmailValidator = this.atLeastOneRequired(complainantPhone, complainantEmail);
    complainantPhone.setValidators(phoneEmailValidator);
    complainantEmail.valueChanges.subscribe(email => {
      complainantPhone.updateValueAndValidity();
    });

    this.updateAnonymousComplainant();

    this.formDataService.getPropertyTypes().subscribe(result => {
      this.propertyTypesStore.dispatch(setPropertyTypes({ propertyTypes: result }));
      this.loaded = true;
    });
  }

  updateAnonymousComplainant() {
    const includeComplainantControl = this.form.get('anonymousComplainant');
    const complainantContactInfoControl = this.form.get('complainantContactInfo');
    const complainantMailingAddressControl = this.form.get('complainantMailingAddress');
    if (includeComplainantControl && complainantContactInfoControl && complainantMailingAddressControl) {
      if (includeComplainantControl.value === true) {
        complainantContactInfoControl.disable();
        complainantMailingAddressControl.disable();
      } else {
        complainantContactInfoControl.enable();
        complainantMailingAddressControl.enable();
      }
    }
  }

  checkTelephoneInvalid() {
    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    return (complainantPhone.touched || complainantEmail.touched) && !complainantPhone.valid;
  }

  save(data: Complaint): Subject<boolean> {
    this.submissionResult = new Subject<boolean>();

    this.submittingForm = this.formDataService.submitCclaForm(data).subscribe({
      error: err => this.submissionResult.error(err)
    });

    return this.submissionResult;
  }

  submit() {
    if (this.form.valid) {
      const formData = this.form.value;
      const data = <Complaint>{
        details: { ...formData.complaintDetails },
        complainant: formData.anonymousComplainant ? null : {
          ...formData.complainantContactInfo,
          address: formData.complainantMailingAddress,
        },
      };

      this.save(data).subscribe({
        error: (err: any) => {
          console.error(err);
          // TODO: display error
          //this.router.navigate(['/error']);
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
