import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../models/Transaction';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  create(transaction: Transaction): Observable<Transaction> {
    return this.http
      .post<Transaction>(`${this.apiUrl}/Transactions/create`, transaction)
      .pipe(catchError(this.handleError));
  }
  
  delete(id: number): Observable<any> {
    return this.http
      .delete<any>(`${this.apiUrl}/Transactions/delete`+id)
      .pipe(catchError(this.handleError));
  }

  getAll(): Observable<Array<Transaction>> {
    return this.http
      .get<Array<Transaction>>(`${this.apiUrl}/Transactions/user`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
