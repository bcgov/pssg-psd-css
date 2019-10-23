import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-complaint-submitted',
  templateUrl: './complaint-submitted.component.html'
})
export class ComplaintSubmittedComponent {
  constructor(private router: Router,
  ) { }
}
