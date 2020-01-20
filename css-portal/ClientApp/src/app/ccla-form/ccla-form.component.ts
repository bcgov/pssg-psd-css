import { Component, OnInit, OnDestroy, ElementRef } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, Subject, Subscription, forkJoin } from 'rxjs';
import { first, filter } from 'rxjs/operators';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { setProvinces } from '@actions/provinces.actions';
import { Complaint } from '@models/complaint.model';
import { Province } from '@models/province.model';
import { Status } from '@models/status.model';
import { ComplaintDataService } from '@services/complaint-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-ccla-form',
  templateUrl: './ccla-form.component.html'
})
export class CclaFormComponent extends FormBase implements OnInit, OnDestroy {
  public provinces: Observable<Province[]>;
  submittingForm: Subscription;
  statusSubscription: Subscription;
  loadingSubscription: Subscription;
  submissionResult: Subject<boolean>;
  loaded: boolean;
  faCalendar = faCalendar;
  authorizationToken : string;
  captchaApiBaseUrl : string;
  zipPostalCodeMask: ((string | RegExp)[] | boolean) = false;

  constructor(
    private formDataService: ComplaintDataService,
    private router: Router,
    private provincesTypesStore: Store<{ provinces: Province[] }>,
    private statusStore: Store<{ status: Status }>,
    private formBuilder: FormBuilder,
    private elementRef: ElementRef,
    private snackBar: MatSnackBar
  ) {
    super();
  }

  ngOnInit() {
    this.form = this.formBuilder.group({
      complaintDetails: this.formBuilder.group({
        name: [''],
        address: this.formBuilder.group({
          unit: [''],
          line1: [''],
          city: ['', Validators.required],
          provinceState: [{ value: 'British Columbia', disabled: true }],
          country: [{ value: 'Canada', disabled: true }],
          zipPostalCode: ['', this.postalCodeValidator],
        }),
        problems: ['', Validators.required],
      }),
      anonymousComplainant: [false],
      complainantContactInfo: this.formBuilder.group({
        firstName: ['', Validators.required],
        middleName: [''],
        lastName: ['', Validators.required],
        fax: ['', this.maskedTelephoneValidator],
        governmentAgency: [''],
        phone: [''],
        email: ['', [Validators.email, this.additionalEmailValidator]],
      }),
      complainantMailingAddress: this.formBuilder.group({
        unit: [''],
        line1: [''],
        city: [''],
        province: [{value: 'British Columbia', disabled: true}, Validators.required],
        provinceState: ['', Validators.required],
        country: [''],
        zipPostalCode: [''],
      }),
    });

    this.setComplainantPhoneEmailValidator();
    this.setComplainantZipPostalCodeValidator();
    this.setComplainantProvinceStateEnabled();
    this.setComplainantCountryValidator();

    this.updateAnonymousComplainant();

    // fetch provinces from back-end and update store
     this.formDataService.getProvinces().subscribe(result => {
      this.provincesTypesStore.dispatch(setProvinces({ provinces: result }));
    });

    // retrieve valid provinces from store
    this.provinces = this.provincesTypesStore.pipe(
      select('provinces'),
      filter(provinces => Array.isArray(provinces))
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

    // set page as loaded once valid provinces and status have been retrieved
    forkJoin([
      this.provinces.pipe(first()),
      statusObservable.pipe(first()),
    ]).subscribe(() => {
      this.loaded = true;
    });
  }

  ngOnDestroy() {
    this.statusSubscription.unsubscribe();
  }

  setComplainantPhoneEmailValidator() {
    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    
    if (complainantPhone && complainantEmail) {
      const phoneEmailValidator = this.atLeastOneRequired(complainantPhone, complainantEmail);
      complainantPhone.setValidators([ phoneEmailValidator, this.maskedTelephoneValidator ]);
      complainantEmail.valueChanges.subscribe(email => {
        complainantPhone.updateValueAndValidity();
      });
    }
  }

  setComplainantZipPostalCodeValidator() {
    const countryControl = this.form.get('complainantMailingAddress.country');
    const zipPostalCodeControl = this.form.get('complainantMailingAddress.zipPostalCode');

    if (countryControl && zipPostalCodeControl) {
      countryControl.valueChanges.subscribe(country => {
        if (country === 'Canada') {
          zipPostalCodeControl.setValidators(this.postalCodeValidator);
          this.zipPostalCodeMask = this.postalCodeMask;
        } else {
          zipPostalCodeControl.setValidators(null);
          this.zipPostalCodeMask = false;
        }
        zipPostalCodeControl.updateValueAndValidity();
      });
    }
  }

  setComplainantProvinceStateEnabled() {
    const countryControl = this.form.get('complainantMailingAddress.country');
    const provinceControl = this.form.get('complainantMailingAddress.province');
    const provinceStateControl = this.form.get('complainantMailingAddress.provinceState');

    if (countryControl && provinceControl && provinceStateControl) {
      countryControl.valueChanges.subscribe(country => {
        if (this.countryIsCanada(country)) {
          provinceControl.enable();
          provinceStateControl.disable();
        } else {
          provinceControl.disable();
          provinceStateControl.enable();
        }
      });
    }
  }

  setComplainantCountryValidator() {
    const countryControl = this.form.get('complainantMailingAddress.country');
    const line1Control = this.form.get('complainantMailingAddress.line1');
    const unitControl = this.form.get('complainantMailingAddress.unit');
    const cityControl = this.form.get('complainantMailingAddress.city');
    const provinceControl = this.form.get('complainantMailingAddress.province');
    const provinceStateControl = this.form.get('complainantMailingAddress.provinceState');
    const zipPostalCodeControl = this.form.get('complainantMailingAddress.zipPostalCode');
    
    countryControl.setValidators(this.requiredIfAnyPopulated(line1Control, unitControl, cityControl, provinceControl, provinceStateControl, zipPostalCodeControl));

    line1Control.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
    unitControl.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
    cityControl.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
    provinceControl.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
    provinceStateControl.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
    zipPostalCodeControl.valueChanges.subscribe(() => countryControl.updateValueAndValidity({ emitEvent: false }));
  }

  checkCountryInvalid() {
    const countryControl = this.form.get('complainantMailingAddress.country');
    const line1Control = this.form.get('complainantMailingAddress.line1');
    const unitControl = this.form.get('complainantMailingAddress.unit');
    const cityControl = this.form.get('complainantMailingAddress.city');
    const provinceControl = this.form.get('complainantMailingAddress.province');
    const provinceStateControl = this.form.get('complainantMailingAddress.provinceState');
    const zipPostalCodeControl = this.form.get('complainantMailingAddress.zipPostalCode');

    return (countryControl.touched || line1Control.touched || unitControl.touched || cityControl.touched ||
      provinceControl.touched || provinceStateControl.touched || zipPostalCodeControl.touched) && !countryControl.valid;
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
      this.snackBar.open('Please Fill All Required Fields', '', {
        duration: 5000,
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: ['alert-danger-snackbar']
      });
      this.scrollToInvalidField();
    }
  }

  scrollToInvalidField() {
    // find invalid input
    let element = this.elementRef.nativeElement.querySelector('.form-control.ng-invalid, .form-check-input.ng-invalid');
    if (!element) {
      return;
    }

    // locate app-field ancestor of input if possible, so label will be visible on screen
    if (Element.prototype.closest) {
      const appField = element.closest('.app-field');
      if (appField) {
        element = appField;
      }
    }

    // scroll to element
    element.scrollIntoView({ behavior: 'smooth' });
  }

  onBusyStop() {
    this.submissionResult.complete();
  }
}
