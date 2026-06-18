import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';

import { setConfig } from '@actions/config.actions';
import { ConfigDataService, ServerConfig } from '@services/config-data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'app';

  constructor(
    private configDataService: ConfigDataService,
    private configStore: Store<{ config: ServerConfig | null }>
  ) { }

  ngOnInit(): void {
    this.configDataService.getServerConfig().subscribe(serverConfig => {
      this.configStore.dispatch(setConfig({ config: serverConfig }));
    });
  }
}
