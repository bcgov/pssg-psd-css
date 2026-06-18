import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Store, select } from "@ngrx/store";
import { filter, take, map } from "rxjs/operators";

import { ServerConfig } from "@services/config-data.service";

@Injectable()
export class CsaDisabledGuard implements CanActivate {
    constructor (
        private router: Router,
        private configStore: Store<{ config: ServerConfig | null }>
    ) { }

    canActivate() {
        return this.configStore.pipe(
            select(state => state.config),
            filter(config => Boolean(config)),
            take(1),
            map(config => {
                if (config?.csaEnabled) {
                    return true;
                } else {
                    return this.router.parseUrl('/');
                }
            }),
        );
    }
}
