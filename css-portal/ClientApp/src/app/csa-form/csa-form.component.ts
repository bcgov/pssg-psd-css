import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, Subject, Subscription } from 'rxjs';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { setPropertyTypes } from '@actions/property-types.actions';
import { Complaint } from '@models/complaint.model';
import { PropertyType } from '@models/property-type.model';
import { ComplaintService } from '@services/form-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-csa-form',
  templateUrl: './csa-form.component.html',
  styleUrls: ['./csa-form.component.scss']
})
export class CsaFormComponent extends FormBase implements OnInit {
  public propertyTypes: Observable<PropertyType[]>;
  submittingForm: Subscription;
  submissionResult: Subject<boolean>;
  loaded: Boolean;
  faCalendar = faCalendar;
  propertyTypeOther = 862570008;

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
        phone: [''],
        fax: [''],
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
    complainantPhone.setValidators(phoneEmailValidator);
    complainantEmail.valueChanges.subscribe(email => {
      complainantPhone.updateValueAndValidity();
    });

    this.formDataService.getPropertyTypes().subscribe(result => {
      this.propertyTypesStore.dispatch(setPropertyTypes({ propertyTypes: result }));
      this.loaded = true;
    });
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
