import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { Account } from '../models/account';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { User } from '../models/user';

@Component({
  selector: 'app-accounts-page',
  templateUrl: './accounts-page.component.html',
  styleUrls: ['./accounts-page.component.scss']
})
export class AccountsPageComponent {

  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
  }); 

  accounts: Account[] = [];
  currentUser: User = {} as User;
  isLoading: boolean = true;


  constructor(private accountService: AccountService, public service: GlobalService, private router: Router,
    private userService: UserService) {
    this.getAccounts();
  }

  getAccounts() {
    this.accountService.getAccounts().subscribe((res) => {
      this.isLoading = false;
      this.accounts = res;
      console.log(this.accounts);
    })
  }

  deleteAccount(account: Account) {
    this.accountService.deleteAccount(account.id).subscribe(res => {
      this.getAccounts();
    })
  }

  editAccount(theaccount: Account) {
    if (this.form.valid) {
      let updatedaccount: Account = {
        id: theaccount.id,
        name: this.form.controls['name'].value,
        balance: this.form.controls['amount'].value,
      } as Account;
      this.accountService.editAccount(updatedaccount.id, updatedaccount).subscribe(res => {
        this.getAccounts();
      })
    }
  }
}
