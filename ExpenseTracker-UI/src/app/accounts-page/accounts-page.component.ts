import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { Account } from '../models/account';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { User } from '../models/user';
import { UserRole } from '../models/userRole';

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

  form2: FormGroup = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    role: new FormControl<string>('', [Validators.required]),
  });
  accounts: Account[] = [];
  currentUser: User = {} as User;
  isLoading: boolean = true;
  email: string = {} as string;
  userRole: UserRole = {} as UserRole;


  constructor(private accountService: AccountService, public service: GlobalService, private router: Router,
    private userService: UserService) {
    this.getAccounts();
  }

  getAccounts() {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
      this.accountService.getAccountsByUser(this.currentUser.id).subscribe((res) => {
        this.isLoading = false;
        this.accounts = res;
        this.fetchUserRolesForAccounts();
      })
    })
  }

  fetchUserRolesForAccounts() {
    this.accounts.forEach(account => {
      this.userService.getUserRoleByUserIdAccountId(account.id, this.currentUser.id).subscribe(
        userRole => {
          account.userRole = userRole; // Add userRole property to each account
        }
      );
    });
  }

  LeaveAccount(accountId: number) {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
      this.accountService.removeUserFromAccount(this.currentUser.id, accountId).subscribe(res => {
        this.getAccounts();
      })
    })
  }

  deleteAccount(account: Account) {
    this.userService.getCurrentUser().subscribe(res => {
      this.accountService.deleteAccount(account.id, res.id).subscribe(res => {
        this.getAccounts();
      })
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

  addUserToAccount(account: Account) {
    console.log(1);
    if (this.form2.valid) {
      this.email = this.form2.controls['email'].value;
      let userrole: UserRole = this.form2.controls['role'].value;
      console.log(this.email);
      console.log(userrole);
      this.userService.getUserByEmail(this.email).subscribe(res => {
        let user = res;
        console.log(user.id);
        console.log(account.id);
        this.accountService.addUserToAccount(account.id, user.id, userrole).subscribe(res => {
          this.getAccounts();
        })
      })
    }
  }

}
