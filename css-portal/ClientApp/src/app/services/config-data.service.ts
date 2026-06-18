import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, shareReplay } from 'rxjs/operators';
import { DataService } from '@services/data.service';

export interface ServerConfig {
  captcha?: {
    key?: string | null;
  };
  csaEnabled?: boolean;
  underMaintenance?: boolean;
}

/**
 * Data service for fetching configuration settings from the API.
 *
 * @export
 * @class ConfigDataService
 * @extends {DataService}
 */
@Injectable({
  providedIn: 'root'
})
export class ConfigDataService extends DataService {
  private config$: Observable<ServerConfig | null> | null = null;

  constructor(private http: HttpClient) {
    super();
  }

  getServerConfig(): Observable<ServerConfig | null> {
    if (!this.config$) {
      this.config$ = this.http
        .get<ServerConfig>(this.apiPath + 'configuration', {
          headers: this.headers
        })
        .pipe(
          shareReplay(1),
          catchError((error) => {
            console.error('Error fetching server configuration', error);
            return of(null);
          })
        );
    }

    return this.config$;
  }
}
