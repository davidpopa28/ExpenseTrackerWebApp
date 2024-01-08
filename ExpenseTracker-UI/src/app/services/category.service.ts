import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Category } from '../models/category';
import { Observable } from 'rxjs';
import { Subcategory } from '../models/subcategory';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseURL: string = "https://localhost:7199/";
  apiPath: string = "api";

  constructor(private httpClient: HttpClient) { }

  getCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(`${this.baseURL}${this.apiPath}/Category`);
  }

  getSubcategoriesByCategory(categoryId: number): Observable<Subcategory[]> {
    return this.httpClient.get<Subcategory[]>(`${this.baseURL}${this.apiPath}/Subcategory/subcategoriesByCategory/${categoryId}`);
  }
}
