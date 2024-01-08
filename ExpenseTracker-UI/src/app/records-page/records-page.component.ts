import { Component } from '@angular/core';
import { Record } from '../models/record';
import { RecordService } from '../services/record.service';
import { UserService } from '../services/user.service';
import { User } from '../models/user';
import { Subscription } from 'rxjs';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { Account } from '../models/account';

@Component({
  selector: 'app-records-page',
  templateUrl: './records-page.component.html',
  styleUrls: ['./records-page.component.scss']
})
export class RecordsPageComponent {

  form: FormGroup = new FormGroup({
    name: new FormControl<string>('', [Validators.required]),
    amount: new FormControl<number>(0, [Validators.required, Validators.minLength(0)]),
  }); 

  accounts : Account []= [];
  records : Record[] =[];
  currentUser: User = {} as User;
  recordsSubscription: Subscription;
  selectedAccountId : number | null = null;
  allaccounts: Account[] =[];

  constructor(private recordService: RecordService, private userService: UserService, private accountService: AccountService) {
    this.recordsSubscription = this.recordService.records$.subscribe(records => {
      if (records && records.length > 0 && records[0].account.id === this.selectedAccountId) {
        this.records = records.reverse();
      }
    });
    this.getAccounts();
  }
  getAccounts() {
    this.userService.getCurrentUser().subscribe(res => {
      this.currentUser=res;
      this.accountService.getAccountsByUser(this.currentUser.id).subscribe((res) => {
        this.accounts = res;
      })
    })
  }

  selectAccount(accountId: number) {
    this.selectedAccountId = accountId;
    this.getRecordsByAccount(accountId);
    console.log(this.records);
  }

  deleteRecord(recordId: number) {
    this.recordService.deleteRecord(recordId).subscribe(res => {
      this.getRecordsByAccount(this.selectedAccountId!);
    })
  }

  getRecordsByAccount(accountId: number) {
    this.recordService.getRecordsByAccount(accountId).subscribe(res => {
      this.records = res;
    })
  }
}
