import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Transaction } from '../models/Transaction';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  create(transaction: Transaction): Observable<Transaction> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .post<Transaction>(`${this.apiUrl}Transactions/create`, transaction, {headers})
      .pipe(catchError(this.handleError));
  }

  update(transaction: Transaction, id:number): Observable<Transaction> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .post<Transaction>(`${this.apiUrl}Transactions/create`, transaction, {headers})
      .pipe(catchError(this.handleError));
  }

  getTransactionsForCategory(startDate: Date, endDate:Date, categoryName:string): Observable<Transaction[]> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .get<Transaction[]>(`${this.apiUrl}Transactions/transactionsInerval/${startDate}/${endDate}/${categoryName}`, {headers})
      .pipe(catchError(this.handleError));
  }
  
  delete(id: number): Observable<any> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .delete<any>(`${this.apiUrl}Transactions/delete/`+id, {headers})
      .pipe(catchError(this.handleError));
  }

  getAll(): Observable<Array<Transaction>> {
    const token = localStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .get<Array<Transaction>>(`${this.apiUrl}Transactions/user`, {headers})
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
