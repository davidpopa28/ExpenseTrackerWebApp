import { Injectable } from '@angular/core';
import {Account} from '../models/account';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { UserRole } from '../models/userRole';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api/Account";


  constructor(private httpClient: HttpClient) { }

  getAccountsByUser(userId: number) : Observable<Account[]> {
    return this.httpClient.get<Account[]>(`${this.baseURL}${this.apiPath}/accountsByUser/${userId}`);
  }

  postAccount(account : Account, userId: number): Observable<Account> {
    return this.httpClient.post<Account>(`${this.baseURL}${this.apiPath}/${userId}`, account);
  }

  deleteAccount(accountId: number, userId: number): Observable<Account> {
    return this.httpClient.delete<Account>(`${this.baseURL}${this.apiPath}/${accountId}/${userId}`);
  }

  editAccount(accountId: number, account:Account): Observable<Account> {
    return this.httpClient.put<Account>(`${this.baseURL}${this.apiPath}/${accountId}`, account);
  }

  addUserToAccount(userId: number, accountId: number, userRole: UserRole): Observable<void> {
    return this.httpClient.post<void>(`${this.baseURL}${this.apiPath}/associateAccount/${userId}/${accountId}/${userRole}`, null);
  }

  removeUserFromAccount(userId:number, accountId: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.baseURL}${this.apiPath}/${userId}/${accountId}`);
  }
}
