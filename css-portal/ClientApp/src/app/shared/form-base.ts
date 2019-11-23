import { FormGroup, ValidatorFn, ValidationErrors, AbstractControl, Validators } from "@angular/forms";

export class FormBase {
  form: FormGroup;
  telephoneMask = ['(', /[2-9]/, /\d/, /\d/, ')', '-', /[2-9]/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  
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

  maskedTelephoneValidator(control: AbstractControl): ValidationErrors | null  {
    const telephoneMaskRegex = /^\([2-9]\d\d\)-[2-9]\d\d-\d\d\d\d$/;
    
    const value = control.value;
    if (value === '') {
      return null;
    } else if (telephoneMaskRegex.test(control.value)) {
      return null;
    } else {
      return { maksedTelephone: true };
    }
  }
}
