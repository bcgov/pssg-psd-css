import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { setStatus } from '@actions/status.actions';
import { Status } from '@models/status.model';
import { StatusDataService } from '@services/status-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(
    private statusDataService: StatusDataService,
    private statusStore: Store<{ status: Status }>
  ) { }

  ngOnInit(): void {
    this.statusDataService.getStatus().subscribe(result => {
      this.statusStore.dispatch(setStatus({ status: result }));
    });
  }
}
