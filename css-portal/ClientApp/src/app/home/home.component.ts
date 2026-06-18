import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Store, select } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServerConfig } from '@services/config-data.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, OnDestroy {
  configSubscription: Subscription | null = null;
  csaEnabled = false;

  constructor(
    private titleService: Title,
    private configStore: Store<{ config: ServerConfig | null }>,
  ) {
    this.titleService.setTitle('Community Safety Unit Complaint Portal');
   }

  ngOnInit(): void {
    this.configSubscription = this.configStore.pipe(
      select(state => state.config),
      map(config => config?.csaEnabled),
    ).subscribe(value => {
      this.csaEnabled = value ?? false;
    });
  }

  ngOnDestroy(): void {
    this.configSubscription?.unsubscribe();
  }
}
