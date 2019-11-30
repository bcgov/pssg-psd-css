import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { StoreModule } from '@ngrx/store';
import { NgBusyModule } from 'ng-busy';
import { TextMaskModule } from 'angular2-text-mask';

import { propertyTypesReducer } from '@reducers/property-types.reducer';
import { statusReducer } from '@reducers/status.reducer';

import { CaptchaDataService } from '@services/captcha-data.service';
import { ComplaintDataService } from '@services/complaint-data.service';
import { StatusDataService } from '@services/status-data.service';

import { FieldComponent } from '@shared/app-field/field.component';
import { CaptchaComponent } from '@shared/captcha/captcha.component';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { CclaFormComponent } from './ccla-form/ccla-form.component';
import { CsaFormComponent } from './csa-form/csa-form.component';
import { ComplaintSubmittedComponent } from './complaint-submitted/complaint-submitted.component';
import { ErrorComponent } from './error/error.component';


@NgModule({
  declarations: [
    CaptchaComponent,
    AppComponent,
    HomeComponent,
    CclaFormComponent,
    CsaFormComponent,
    ComplaintSubmittedComponent,
    ErrorComponent,
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
    TextMaskModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'ccla-form', component: CclaFormComponent },
      { path: 'csa-form', component: CsaFormComponent },
      { path: 'complaint-submitted', component: ComplaintSubmittedComponent },
      { path: 'error', component: ErrorComponent },
      { path: '**', redirectTo: '' },
    ]),
    StoreModule.forRoot({ propertyTypes: propertyTypesReducer, status: statusReducer })
  ],
  providers: [
    CaptchaDataService,
    ComplaintDataService,
    StatusDataService,
    Title
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
