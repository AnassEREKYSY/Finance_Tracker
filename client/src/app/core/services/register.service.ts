import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Register } from '../models/Register';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.developement';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  register(register: Register): Observable<Register> {
    return this.http
      .post<Register>(`${this.apiUrl}Users/register`, register)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
