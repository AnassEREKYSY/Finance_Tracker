import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
      .post<Category>(`${this.apiUrl}Categories/create`, category)
      .pipe(catchError(this.handleError));
  }

  getAll(): Observable<Category[]> {
    const token = sessionStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    
    return this.http
      .get<Category[]>(`${this.apiUrl}Categories/getAll`, { headers })
      .pipe(catchError(this.handleError));
  }

  delete(id:number): Observable<any> {
    return this.http
      .delete<any>(`${this.apiUrl}Categories/delete/`+id)
      .pipe(catchError(this.handleError));
  }

  update(id:number, category:Category): Observable<Category> {
    return this.http
      .put<Category>(`${this.apiUrl}Categories/update/`+id, category)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
