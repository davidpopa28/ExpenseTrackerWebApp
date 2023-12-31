import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserLogin } from '../models/userLogin';
import { Observable } from 'rxjs';
import { Token } from '../models/token';

@Injectable({
  providedIn: 'root'
})
export class LoginserviceService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api/Authentification/login";
   
  constructor(private httpClient: HttpClient) { }

  login(userLogin: UserLogin): Observable<Token> {
    return this.httpClient.post<Token>(`${this.baseURL}${this.apiPath}`, userLogin);
  }
}
