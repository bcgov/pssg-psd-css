import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Store, select } from "@ngrx/store";
import { filter, take, map } from "rxjs/operators";

import { Status } from "@models/status.model";

@Injectable()
export class MaintenanceGuard implements CanActivate {
    constructor (
        private router: Router,
        private statusStore: Store<{ status: Status }>
    ) { }
    
    canActivate() {
        return this.statusStore.pipe(
            select(state => state.status),
            filter(status => Boolean(status)),
            take(1),
            map(status => {
                if (status.underMaintenance) {
                    return this.router.parseUrl('/under-maintenance');
                }
                else {
                    return true;
                }
            }),
        );
    }
}
