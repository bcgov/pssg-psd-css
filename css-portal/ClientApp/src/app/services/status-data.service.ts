import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { DataService } from './data.service';

import { Status } from '@models/status.model';

@Injectable()
export class StatusDataService extends DataService {

  constructor(private http: HttpClient) {
    super();
  }

  /**
   * Retrieve status from back-end
   */
  getStatus(): Observable<Status> {
    const path = `${this.apiPath}/status/`;
    return this.http.get<Status>(path, { headers: this.headers })
      .pipe(catchError(this.handleError));
  }
}
