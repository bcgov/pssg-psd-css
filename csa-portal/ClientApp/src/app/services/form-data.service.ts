import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { DataService } from './data.service';

import { Activity } from '@models/activity.model';

@Injectable()
export class FormDataService extends DataService {

  constructor(private http: HttpClient) {
    super();
  }

  /**
   * Retrieve activities from back-end
   */
  getActivities(): Observable<Activity[]> {
    const path = `${this.apiPath}/form/activities`;
    return this.http.get<Activity[]>(path, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }

  /**
   * Submit form to back-end
   * @param data - form data
   */
  submitForm(data: any) {
    const path = `${this.apiPath}/form/`;
    return this.http.post<any>(path, data, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }
}
