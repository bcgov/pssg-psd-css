import { Component, OnInit, OnDestroy, ElementRef } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Subject, Subscription, forkJoin } from 'rxjs';
import { first, filter } from 'rxjs/operators';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { Complaint } from '@models/complaint.model';
import { Status } from '@models/status.model';
import { ComplaintDataService } from '@services/complaint-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-ccla-form',
  templateUrl: './ccla-form.component.html'
})
export class CclaFormComponent extends FormBase implements OnInit, OnDestroy {
  submittingForm: Subscription;
  statusSubscription: Subscription;
  loadingSubscription: Subscription;
  submissionResult: Subject<boolean>;
  loaded: boolean;
  faCalendar = faCalendar;
  authorizationToken : string;
  captchaApiBaseUrl : string;

  constructor(
    private formDataService: ComplaintDataService,
    private router: Router,
    private statusStore: Store<{ status: Status }>,
    private formBuilder: FormBuilder,
    private elementRef: ElementRef
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
        fax: ['', this.maskedTelephoneValidator],
        governmentAgency: [''],
        phone: [''],
        email: ['', Validators.email],
      }),
      complainantMailingAddress: this.formBuilder.group({
        unit: [''],
        line1: [''],
        city: [''],
        provinceState: [],
        country: [],
        zipPostalCode: [''],
      }),
    });

    const complainantPhone = this.form.get('complainantContactInfo.phone');
    const complainantEmail = this.form.get('complainantContactInfo.email');
    const phoneEmailValidator = this.atLeastOneRequired(complainantPhone, complainantEmail);
    complainantPhone.setValidators([ phoneEmailValidator, this.maskedTelephoneValidator ]);
    complainantEmail.valueChanges.subscribe(email => {
      complainantPhone.updateValueAndValidity();
    });

    this.updateAnonymousComplainant();

    // retrieve valid status from store
    const statusObservable =  this.statusStore.pipe(
      select(state => state.status),
      filter(status => Boolean(status && status.captchaApiUrl))
    );

    // retrieve captcha api URL from status
    this.statusSubscription = statusObservable.subscribe(status => {
      this.captchaApiBaseUrl = status.captchaApiUrl;
    });

    // set page as loaded once a valid status has been retrieved
    statusObservable.pipe(first()).subscribe(() => {
      this.loaded = true;
    });
  }

  ngOnDestroy() {
    this.statusSubscription.unsubscribe();
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
