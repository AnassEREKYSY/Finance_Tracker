import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.developement';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Notification } from '../models/Notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {

  private apiUrl = environment.apiUrl;
   constructor(private http: HttpClient) {}
 
  create(notification: Notification): Observable<Notification> {
     const token = sessionStorage.getItem('authToken');
     const headers = new HttpHeaders({
       Authorization: `Bearer ${token}`,
     });
     return this.http
       .post<Notification>(`${this.apiUrl}Notifications/create`, notification, { headers })
       .pipe(catchError(this.handleError));
   }

  getAll(): Observable<Notification[]> {
    const token = sessionStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .get<Notification[]>(`${this.apiUrl}Notifications/user`, { headers })
      .pipe(catchError(this.handleError));
  }

  markAsRead(id:number): Observable<Boolean> {
    const token = sessionStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .put<Boolean>(`${this.apiUrl}Notifications/asRead/`+id, {},{ headers })
      .pipe(catchError(this.handleError));
  }

  delete(id:number): Observable<Boolean> {
    const token = sessionStorage.getItem('authToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    return this.http
      .delete<Boolean>(`${this.apiUrl}Notifications/delete/`+id,{ headers })
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<never> {
       console.error('Error occurred:', error);
       return throwError(() => new Error('Something went wrong.'));
  }
}
