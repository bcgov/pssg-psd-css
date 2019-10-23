import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
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
  selector: 'app-csa-form',
  templateUrl: './csa-form.component.html',
  providers: [
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
  ]
})
export class CsaFormComponent extends FormBase implements OnInit {
  public propertyTypes: Observable<PropertyType[]>;
  form: FormGroup;
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
      property: this.formBuilder.group({
        name: [''],
        address: this.formBuilder.group({
          unit: [''],
          line1: [''],
          line2: [''],
          city: ['', Validators.required],
          provinceState: [{ value: 'BC', disabled: true }],
          country: [{ value: 'Canada', disabled: true }],
          zipPostalCode: [''],
        }),
        propertyType: [''],
        otherPropertyType: [''],
        description: ['', Validators.required],
        problems: ['', Validators.required],
        occupantName: [''],
        ownerName: [''],
      }),
      includeComplainant: [false],
      complainant: this.formBuilder.group({
        firstName: ['', Validators.required],
        middleName: [''],
        lastName: ['', Validators.required],
        phone: ['', Validators.required],
        fax: [''],
        email: ['', [Validators.email, Validators.required]],
        address: this.formBuilder.group({
          unit: [''],
          line1: ['', Validators.required],
          line2: [''],
          city: ['', Validators.required],
          provinceState: ['BC', Validators.required],
          country: ['Canada', Validators.required],
          zipPostalCode: ['', Validators.required],
        }),
      }),
    });
    
    this.updateComplainantEnabled();

    this.formDataService.getPropertyTypes().subscribe(result => {
      this.propertyTypesStore.dispatch(setPropertyTypes({ propertyTypes: result }));
      this.loaded = true;
    });
  }

  save(data: Complaint): Subject<boolean> {
    this.submissionResult = new Subject<boolean>();

    this.submittingForm = this.formDataService.submitCsaForm(data).subscribe(
      undefined,
      err => this.submissionResult.error(err)
    );

    return this.submissionResult;
  }

  submit() {
    if (this.form.valid) {
      const data = <Complaint>{
        ...this.form.value,
      };
      debugger;

      this.save(data).subscribe(
        undefined,
        (err: any) => {
          console.error(err);
          // TODO: display error
          //this.router.navigate(['/error']);
        },
        () => {
          this.router.navigate(['/complaint-submitted']);
        }
      );
    } else {
      this.form.markAllAsTouched();
    }
  }

  onBusyStop() {
    this.submissionResult.complete();
  }

  updateComplainantEnabled() {
    const includeComplainantControl = this.form.get('includeComplainant');
    const complainantControl = this.form.get('complainant');
    if (includeComplainantControl && complainantControl) {
      if (includeComplainantControl.value === true) {
        complainantControl.enable();
      } else {
        complainantControl.disable();
      }
    }
  }
}
