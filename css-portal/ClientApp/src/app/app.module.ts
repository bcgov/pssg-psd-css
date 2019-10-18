import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { StoreModule } from '@ngrx/store';
import { NgBusyModule } from 'ng-busy';

import { activitiesReducer } from '@reducers/activities.reducer';
import { FormDataService } from '@services/form-data.service';

import { FieldComponent } from '@shared/app-field/field.component';

import { AppComponent } from './app.component';
import { FormComponent } from './form/form.component';
import { FormSubmittedComponent } from './form-submitted/form-submitted.component';


@NgModule({
  declarations: [
    AppComponent,
    FormComponent,
    FormSubmittedComponent,
    FieldComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    FontAwesomeModule,
    BrowserAnimationsModule,
    NgBusyModule,
    RouterModule.forRoot([
      { path: '', component: FormComponent, pathMatch: 'full' },
      { path: 'form-submitted', component: FormSubmittedComponent },
    ]),
    StoreModule.forRoot({ activities: activitiesReducer })
  ],
  providers: [
    FormDataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
