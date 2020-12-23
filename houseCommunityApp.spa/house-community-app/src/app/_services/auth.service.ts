import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../Model/user';
import { Observable, of, throwError } from 'rxjs';
import { Role } from '../Model/Role';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  user: User;
  constructor(private http: HttpClient) {
  }

  isAuthorized() {
    return !!this.user;
}

hasRole(role: Role) {
    return this.isAuthorized() && this.user.role === role;
}

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('role', user.user.userRole);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.user = new User();
          this.user.token = user.token;
          this.user.role = user.user.userRole;
          console.log(this.user);
        return user.user;
        }
      }
    ,catchError(err => {
      return throwError(err);
  })));
  }

  registerUser(model: any) {
    return this.http.post(this.baseUrl + 'register', model).pipe(catchError(err => {
      return throwError(err);
  }));
  }
  requestReset(body): Observable<any> {
    return this.http.post(`${this.baseUrl}req-reset-password`, body).pipe(catchError(err => {
      return throwError(err);
  }));
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  newPassword(body): Observable<any> {
    return this.http.post(`${this.baseUrl}new-password`, body).pipe(catchError(err => {
      return throwError(err);
  }));
  }

  changePassword(body): Observable<any> {
    return this.http.post(`${this.baseUrl}change-password`, body).pipe(catchError(err => {
      return throwError(err);
  }),);
  }
  
  handleError(error: HttpErrorResponse) {
    return throwError(error);
}

  ValidPasswordToken(body): Observable<any> {
    return this.http.post(`${this.baseUrl}valid-password-token`, body);
  }
}