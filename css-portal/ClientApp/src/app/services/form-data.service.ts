import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { DataService } from './data.service';

import { PropertyType } from '@models/property-type.model';

@Injectable()
export class ComplaintService extends DataService {

  constructor(private http: HttpClient) {
    super();
  }

  /**
   * Retrieve property types from back-end
   */
  getPropertyTypes(): Observable<PropertyType[]> {
    const path = `${this.apiPath}/complaints/property-types`;
    return this.http.get<PropertyType[]>(path, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Submit CSA form to back-end
   * @param data - form data
   */
  submitCsaForm(data: any) {
    const path = `${this.apiPath}/complaints/csa`;
    return this.http.post<any>(path, data, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Submit CCLA form to back-end
   * @param data - form data
   */
  submitCclaForm(data: any) {
    const path = `${this.apiPath}/complaints/ccla`;
    return this.http.post<any>(path, data, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }
}
