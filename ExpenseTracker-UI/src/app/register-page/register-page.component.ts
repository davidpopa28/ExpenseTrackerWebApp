import { Component } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {

  MatchingValidator: ValidatorFn = (form: AbstractControl): ValidationErrors | null => {
    return (formGroup: FormGroup) => {
      const control = form.get("password");
      const matchingControl = form.get("confirmPassword");
      
      if(control && matchingControl && control.value != matchingControl.value) {
          return { passwordmatcherror: true };
      }
      return null;
    };
  }

  passwordValidator: ValidatorFn = ( control: AbstractControl ): ValidationErrors | null => {
      const password = control.value;
      const hasLower = /[a-z]/.test(password);
      const hasUpper = /[A-Z]/.test(password);
      const hasNumber = /\d/.test(password);
      //const hasSymbol = '!@#$%^&*()-_=+'.
      
      const isValid = hasLower && hasUpper && hasNumber;
      if (!isValid) {
          return {
              lower: !hasLower,
              upper: !hasUpper,
              number: !hasNumber
          }
      }
      return null;
  };
  
  form: FormGroup = new FormGroup({
    firstName: new FormControl<string>('', [Validators.required]),
    lastName: new FormControl<string>('', [Validators.required]),
    nickName: new FormControl<string>('', [Validators.required]),
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    phone: new FormControl<string>('', [Validators.required]),
    password: new FormControl<string>('', [Validators.required, Validators.minLength(8), this.passwordValidator]),
    confirmPassword: new FormControl<string>('', [Validators.required])
  }, [this.MatchingValidator]);

  constructor() { }

  onSubmit() {
  }
}