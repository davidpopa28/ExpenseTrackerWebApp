import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalService {

  baseURL: string = "https://localhost:7199";
  private search: string = '';
  isLoggedIn = false;

  constructor() { }

  searchChange: Subject<string> = new Subject<string>();

  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms) );
  }

  setLogIn(isLoggedIn: boolean) {
    this.isLoggedIn = isLoggedIn;
  }
}