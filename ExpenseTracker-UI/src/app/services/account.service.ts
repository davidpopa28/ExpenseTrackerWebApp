import { Injectable } from '@angular/core';
import {Account} from '../models/account';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api/Account";


  constructor(private httpClient: HttpClient) { }

  getAccounts() : Observable<Account[]> {
    return this.httpClient.get<Account[]>(`${this.baseURL}${this.apiPath}`);
  }

  postAccount(account : Account, userId: number): Observable<Account> {
    return this.httpClient.post<Account>(`${this.baseURL}${this.apiPath}/${userId}`, account);
  }

  deleteAccount(accountId: number): Observable<Account> {
    return this.httpClient.delete<Account>(`${this.baseURL}${this.apiPath}/${accountId}`);
  }

  editAccount(accountId: number, account:Account): Observable<Account> {
    return this.httpClient.put<Account>(`${this.baseURL}${this.apiPath}/${accountId}`, account);
  }
}
