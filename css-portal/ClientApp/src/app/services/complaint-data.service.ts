import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { DataService } from './data.service';

import { PropertyType } from '@models/property-type.model';
import { Province } from '@models/province.model';

@Injectable()
export class ComplaintDataService extends DataService {

  constructor(private http: HttpClient) {
    super();
  }

  /**
   * Retrieve property types from back-end
   */
  getPropertyTypes(): Observable<PropertyType[]> {
    const path = this.apiPath + 'complaints/property-types';
    return this.http.get<PropertyType[]>(path, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Retrieve provinces from back-end
   */
  getProvinces(): Observable<Province[]> {
    const path = this.apiPath + 'complaints/provinces';
    return this.http.get<Province[]>(path, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Submit CSA form to back-end
   * @param data - form data
   */
  submitCsaForm(data: any) {
    const path = this.apiPath + 'complaints/csa';
    return this.http.post<any>(path, data, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Submit CCLA form to back-end
   * @param data - form data
   */
  submitCclaForm(data: any) {
    const path = this.apiPath + 'complaints/ccla';
    return this.http.post<any>(path, data, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }
}
