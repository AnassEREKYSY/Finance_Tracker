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

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
