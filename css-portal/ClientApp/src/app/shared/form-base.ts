import { FormGroup, ValidatorFn, ValidationErrors, AbstractControl, Validators } from "@angular/forms";

export class FormBase {
  form: FormGroup;
  telephoneMask = ['(', /[2-9]/, /\d/, /\d/, ')', '-', /[2-9]/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  postalCodeMask = [/[a-zA-Z]/, /\d/, /[a-zA-Z]/, ' ', /\d/, /[a-zA-Z]/, /\d/];
  postalCodeValidator = Validators.pattern(/^[a-zA-Z]\d[a-zA-Z] ?\d[a-zA-Z]\d$/);
  additionalEmailValidator = Validators.pattern(/^.+@.+\..+$/); // ensure domain has dot
  
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

  requiredIfAnyPopulated(...controls: AbstractControl[]): ValidatorFn {
    return (thisControl: AbstractControl): ValidationErrors | null => {
      for (const control of controls) {
        if (!Validators.required(control) && control.enabled) { // if any are non-empty, this field is required
          return Validators.required(thisControl);
        }
      }
      return null;
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
      return { maskedTelephone: true };
    }
  }

  uppercaseMaskPipe(conformedValue: string) {
    return conformedValue.toUpperCase();
  }

  countryIsCanada(country: string): boolean {
    return country && country.toUpperCase() === "CANADA";
  }
}
