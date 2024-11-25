import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Category } from '../models/Category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  create(category: Category): Observable<Category> {
    return this.http
      .post<Category>(`${this.apiUrl}/Categories/create`, category)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
