import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-under-maintenance',
  templateUrl: './under-maintenance.component.html'
})
export class UnderMaintenanceComponent {
  constructor(
    private titleService: Title
  ) {
    this.titleService.setTitle('Community Safety Unit Complaint Portal Under Maintenance');
   }
}
