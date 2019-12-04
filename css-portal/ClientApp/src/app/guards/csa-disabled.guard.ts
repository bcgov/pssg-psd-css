import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Store, select } from "@ngrx/store";
import { filter, take, map } from "rxjs/operators";

import { Status } from "@models/status.model";

@Injectable()
export class CsaDisabledGuard implements CanActivate {
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
                if (status.csaEnabled) {
                    return true;
                } else {
                    return this.router.parseUrl('/');
                }
            }),
        );
    }
}
