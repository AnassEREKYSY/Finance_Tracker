import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Login } from '../models/Login';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  login(login: Login): Observable<Login> {
    return this.http
      .post<Login>(`${this.apiUrl}/Users/login`, login)
      .pipe(catchError(this.handleError));
  }

  logOut(): Observable<any> {
    return this.http
      .post<any>(`${this.apiUrl}/Users/logout`, {})
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
    console.error('Error occurred:', error);
    return throwError(() => new Error('Something went wrong.'));
  }
}
