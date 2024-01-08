import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { UserRole } from '../models/userRole';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api/User";

  constructor(private httpClient: HttpClient) { }

  getCurrentUser(): Observable<User> {
    return this.httpClient.get<User>(`${this.baseURL}${this.apiPath}/current`);
  }

  getUserByEmail(email: string): Observable<User> {
    return this.httpClient.get<User>(`${this.baseURL}${this.apiPath}/getUserByEmail/${email}`);
  }

  getUserRoleByUserIdAccountId(accountId: number, userId: number): Observable<UserRole> {
    return this.httpClient.get<UserRole>(`${this.baseURL}${this.apiPath}/userRole/${accountId}/${userId}`);
  }
}
