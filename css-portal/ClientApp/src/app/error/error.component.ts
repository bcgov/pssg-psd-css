import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html'
})
export class ErrorComponent {
  constructor(
    private titleService: Title
  ) {
    this.titleService.setTitle('Community Safety Unit Complaint Portal Error');
   }
}
