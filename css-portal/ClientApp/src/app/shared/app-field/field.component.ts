import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-field',
  templateUrl: './field.component.html'
})
export class FieldComponent {
  @Input() required = false;
  @Input() invalid = false;
  @Input() label: string;
  @Input() errorMessage: string;

  constructor() {}
}
