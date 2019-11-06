import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-field',
  templateUrl: './field.component.html'
})
export class FieldComponent implements OnInit {
  @Input() required = false;
  @Input() invalid = false;
  @Input() label: string;
  @Input() errorMessage: string;

  constructor() { }

  ngOnInit() {
  }

}
