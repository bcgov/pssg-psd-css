import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Store, select } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

import { Status } from '@models/status.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy {
  csaEnabled = false;
  statusSubscription: Subscription;

  constructor(
    private titleService: Title,
    private statusStore: Store<{ status: Status }>,
  ) {
    this.titleService.setTitle('Community Safety Unit Complaint Portal');
   }

  ngOnInit(): void {
    this.statusSubscription = this.statusStore.pipe(
      select(state => state.status),
      map(status => status && status.csaEnabled),
    ).subscribe(value => {
      this.csaEnabled = value;
    });
  }
  
  ngOnDestroy(): void {
    this.statusSubscription.unsubscribe();
  }
}
