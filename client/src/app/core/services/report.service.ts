import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.developement';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  generateBudgetReport(budgetIds: number[]): Observable<Blob> {
    const token = sessionStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http
      .post<Blob>(`${this.apiUrl}Report/generate-budget-report`, budgetIds, { 
        headers, 
        responseType: 'blob' as 'json' 
      })
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
