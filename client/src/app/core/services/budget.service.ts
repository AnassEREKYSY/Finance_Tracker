import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient } from '@angular/common/http';
import { Budget } from '../models/Budget';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  create(budget: Budget): Observable<Budget> {
    return this.http
      .post<Budget>(`${this.apiUrl}/Budgets/create`, budget)
      .pipe(catchError(this.handleError));
  }

  update(updatedBudget: Budget, id: string): Observable<Budget> {
    return this.http
      .post<Budget>(`${this.apiUrl}/Budgets/update/${id}`, updatedBudget)
      .pipe(catchError(this.handleError));
  }

  delete(id: number): Observable<any> {
    return this.http
      .delete<any>(`${this.apiUrl}/Budgets/delete`+id)
      .pipe(catchError(this.handleError));
  }

  getAll(): Observable<Array<Budget>> {
    return this.http
      .get<Array<Budget>>(`${this.apiUrl}/Budgets/getAll`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
