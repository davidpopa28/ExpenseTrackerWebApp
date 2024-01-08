import { Component, OnInit } from '@angular/core';
import { Account } from '../models/account';
import { AccountService } from '../services/account.service';
import { GlobalService } from '../services/global.service';
import { Router } from '@angular/router';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Record } from '../models/record';
import { RecordService } from '../services/record.service';
import { BehaviorSubject, Observable, Subscription, forkJoin, switchMap } from 'rxjs';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent{
  accounts: Account[] = [];
  records: Record[] = [];
  expenseRecords: Record[] = [];
  incomeRecords: Record[] = [];
  isLoading: boolean = true;
  currentUser: User = {} as User;
  topRecords: Record[] = [];

  //recordsSubscription: Subscription;
  recordsToHomePageSubscription: Subscription;


  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
  });

  constructor(private accountService: AccountService, public service: GlobalService, private router: Router,
    private userService: UserService, private recordService: RecordService) {
    this.recordsToHomePageSubscription = this.recordService.recordsToHomePage$.subscribe(records => {
      this.records=records.reverse();

      this.incomeRecords = records.filter(record => record.type === 'Income');
      this.expenseRecords = records.filter(record => record.type === 'Expense');

      this.incomeRecords.sort((a, b) => b.value - a.value);
      this.expenseRecords.sort((b, a) => b.value - a.value);
    });
    this.getAccountsByUser();
  }


  getAccountsByUser() {
    this.isLoading=true;
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser = res;
      this.accountService.getAccountsByUser(this.currentUser.id).subscribe((res) => {
        this.isLoading = false;
        this.accounts = res;
      })

      this.recordService.getRecordsByUser(this.currentUser.id).subscribe(res => {
        this.records = res;

        this.incomeRecords = res.filter(record => record.type === 'Income');
        this.expenseRecords = res.filter(record => record.type === 'Expense');

        this.incomeRecords.sort((a, b) => b.value - a.value);
        this.expenseRecords.sort((b, a) => b.value - a.value);
        this.isLoading=false;
      })
    })
  }

  addAccount() {
    this.isLoading=true;
    if (this.form.valid) {
      let account: Account = {
        name: this.form.controls['name'].value,
        balance: this.form.controls['amount'].value,
      } as Account;
      this.userService.getCurrentUser().subscribe(res => {
        this.currentUser = res;
        this.accountService.postAccount(account, this.currentUser.id).subscribe(res => {
          this.getAccountsByUser()
          this.isLoading=false;
        })
      })
    }
    else {
      alert('enter valid inputs');
    }
  }

  logout() {
    localStorage.removeItem("token");
    this.router.navigate(['/login']);
  }
}
