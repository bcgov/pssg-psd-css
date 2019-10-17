import { FormGroup } from "@angular/forms";

export class FormBase {
  form: FormGroup;
  
  checkInputInvalid(controlName: string) {
    let control = this.form.get(controlName);
    return control.touched && !control.valid;
  }
}
