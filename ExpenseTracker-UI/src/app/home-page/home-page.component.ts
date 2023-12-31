import { Component } from '@angular/core';
import { Account } from '../models/account';
import { AccountService } from '../services/account.service';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';
import { FormControl, Validators, FormGroup} from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent {
  accounts: Account[] = [];
  isLoading: boolean = true;
  currentUser : User = {} as User;

  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
  });

  constructor(private accountService: AccountService, public service: GlobalService, private router: Router,
    private userService: UserService) {
    this.getAccounts();
  }

  getAccounts() {
    this.accountService.getAccounts().subscribe((res) => {
      this.isLoading = false;
      this.accounts = res;
    })
  }

  addAccount() {
    if(this.form.valid) {
      let account: Account = {
        name : this.form.controls['name'].value,
        balance : this.form.controls['amount'].value,
      } as Account;
      this.userService.getCurrentUser().subscribe(res => {
        this.currentUser=res;
      console.log(this.currentUser);
      this.accountService.postAccount(account, this.currentUser.id).subscribe(res => {
        this.getAccounts()
        this.router.navigate(['/home'])
      })
    })
    }
    else {
      alert('enter valid inputs');
    }
  }

}
