import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Record } from '../models/record';
import { RecordDTO } from '../models/recordDTO';
import { BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecordService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api/Record";

  constructor(private httpClient: HttpClient) { }

  private recordsSubject = new BehaviorSubject<Record[]>([]);
  private recordsToHomePageSubject = new BehaviorSubject<Record[]>([]);
  public recordsToHomePage$: Observable<Record[]> =this.recordsToHomePageSubject.asObservable();
  public records$: Observable<Record[]> = this.recordsSubject.asObservable();

  getRecordsByUser(userId: number): Observable<Record[]> {
    return this.httpClient.get<Record[]>(`${this.baseURL}${this.apiPath}/recordsByUser/${userId}`)
      .pipe(tap(records => this.recordsToHomePageSubject.next(records)));
  }

  addRecord(userId: number, accountId: number, subcategoryId: number, record: RecordDTO): Observable<RecordDTO> {
    return this.httpClient.post<RecordDTO>(`${this.baseURL}${this.apiPath}/${userId}/${accountId}/${subcategoryId}`, record)
      .pipe(tap(() => this.getRecordsByAccount(accountId).subscribe()));
  }

  deleteRecord(recordId: number): Observable<Record> {
    return this.httpClient.delete<Record>(`${this.baseURL}${this.apiPath}/${recordId}`);
  }

  getRecordsByAccount(accountId: number): Observable<Record[]> {
    return this.httpClient.get<Record[]>(`${this.baseURL}${this.apiPath}/recordsByAccount/${accountId}`)
      .pipe(tap(records=>this.recordsSubject.next(records)));
  }

  getRecordsBySubcategoriesAndUser(subcategoryId: number, userId: number): Observable<Record[]> {
    return this.httpClient.get<Record[]>(`${this.baseURL}${this.apiPath}/recordsBySubcategory/${subcategoryId}/${userId}`);
  }
}
