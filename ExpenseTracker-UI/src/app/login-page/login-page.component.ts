import { Component } from '@angular/core';
import { FormControl, Validators, FormGroup} from '@angular/forms';
import { UserLogin } from '../models/userLogin';
import { LoginserviceService } from '../services/loginservice.service';
import { Token } from '../models/token';
import { Router } from '@angular/router';
import { GlobalService } from '../services/global.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent {

  form: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', [Validators.required, Validators.minLength(6)]),
  });

  constructor(private loginService : LoginserviceService,
    private service: GlobalService,
    private router: Router) { }

  login() {
    let userLogin: UserLogin = {
      email: this.form.controls['email'].value,
      password: this.form.controls['password'].value,
    }    
    this.loginService.login(userLogin).subscribe((res: Token) => {
      localStorage.setItem("token", res.token);
      this.service.setLogIn(true);
      this.router.navigate(['/home']);
    })
  }
}
