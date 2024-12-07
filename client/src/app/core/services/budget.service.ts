import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Budget } from '../models/Budget';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  create(budget: Budget): Observable<Budget> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .post<Budget>(`${this.apiUrl}Budgets/create`, budget, { headers })
      .pipe(catchError(this.handleError));
  }

  update(updatedBudget: Budget, id: string): Observable<Budget> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .post<Budget>(`${this.apiUrl}Budgets/update/${id}`, updatedBudget, { headers })
      .pipe(catchError(this.handleError));
  }

  delete(id: number): Observable<any> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .delete<any>(`${this.apiUrl}Budgets/delete/`+id, { headers })
      .pipe(catchError(this.handleError));
  }

  getAll(): Observable<Array<Budget>> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  
    return this.http
      .get<Array<Budget>>(`${this.apiUrl}Budgets/getAll`, { headers })
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
