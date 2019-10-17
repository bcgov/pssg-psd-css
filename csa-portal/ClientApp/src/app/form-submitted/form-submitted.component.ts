import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-form-submitted',
  templateUrl: './form-submitted.component.html'
})
export class FormSubmittedComponent {
  constructor(private router: Router,
  ) { }
}
