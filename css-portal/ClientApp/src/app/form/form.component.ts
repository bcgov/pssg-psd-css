import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { Observable, Subject, Subscription } from 'rxjs';
import { NgbDateAdapter, NgbDateNativeAdapter } from '@ng-bootstrap/ng-bootstrap';
import { faCalendar } from '@fortawesome/free-solid-svg-icons';

import { setActivities } from '@actions/activities.actions';
import { Activity } from '@models/activity.model';
import { FormData } from '@models/form-data.model';
import { FormDataService } from '@services/form-data.service';
import { FormBase } from '@shared/form-base';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  providers: [
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
  ]
})
export class FormComponent extends FormBase implements OnInit {
  public activities: Observable<Activity[]>;
  form: FormGroup;
  submittingForm: Subscription;
  submissionResult: Subject<boolean>;
  loaded: Boolean;
  faCalendar = faCalendar;

  constructor(
    private formDataService: FormDataService,
    private router: Router,
    private activitiesStore: Store<{ activities: Activity[] }>,
    private formBuilder: FormBuilder
  ) {
    super();
  }

  ngOnInit() {
    this.activities = this.activitiesStore.pipe(select('activities'));

    this.form = this.formBuilder.group({
      activity: ['', Validators.required],
      location: ['', Validators.required],
      details: [''],
      date: [null],
    });

    this.formDataService.getActivities().subscribe(result => {
      this.activitiesStore.dispatch(setActivities({ activities: result }));
      this.loaded = true;
    });
  }

  // marking the form as touched makes the validation messages show
  markAsTouched() {
    this.form.markAsTouched();

    const controls = this.form.controls;
    for (const c in controls) {
      if (typeof (controls[c].markAsTouched) === 'function') {
        controls[c].markAsTouched();
      }
    }
  }

  save(data: FormData): Subject<boolean> {
    this.submissionResult = new Subject<boolean>();

    this.submittingForm = this.formDataService.submitForm(data).subscribe(
      undefined,
      err => this.submissionResult.error(err)
    );

    return this.submissionResult;
  }

  submit() {
    if (this.form.valid) {
      const data = <FormData>{
        ...this.form.getRawValue(),
      };

      this.save(data).subscribe(
        undefined,
        (err: any) => {
          console.error(err);
          // TODO: display error
          //this.router.navigate(['/error']);
        },
        () => {
          this.router.navigate(['/form-submitted']);
        }
      );
    } else {
      this.markAsTouched();
    }
  }

  onBusyStop() {
    this.submissionResult.complete();
  }
}
