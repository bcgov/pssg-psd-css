import { FormGroup, ValidatorFn, ValidationErrors, AbstractControl, Validators } from "@angular/forms";

export class FormBase {
  form: FormGroup;
  
  checkInputInvalid(controlName: string) {
    let control = this.form.get(controlName);
    return control.touched && !control.valid;
  }

  atLeastOneRequired(...controls: AbstractControl[]): ValidatorFn {
    return (thisControl: AbstractControl): ValidationErrors | null => {
      for (const control of controls) {
        if (!Validators.required(control)) { // if any are non-empty, validation succeeds
          return null;
        }
      }
      return { oneRequired: true };
    };
  }
}
