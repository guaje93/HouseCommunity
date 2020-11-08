import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { catchError, map } from "rxjs/operators";
import { JwtHelperService } from "@auth0/angular-jwt";
import { User } from '../Model/user';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = "http://localhost:5000/api/auth/";
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  constructor(private http: HttpClient) {
  }

  login(model: any) {
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem("token", user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
        return user.user;
        }
      })
    );
  }
  requestReset(body): Observable<any> {
    return this.http.post(`${this.baseUrl}req-reset-password`, body);
  }

  loggedIn() {
    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);
  }

  newPassword(body): Observable<any> {
    return this.http.post(`${this.baseUrl}new-password`, body);
  }

  changePassword(body): Observable<any> {
    return this.http.post(`${this.baseUrl}change-password`, body).pipe(catchError(this.handleError));
  }
  
  handleError(error: HttpErrorResponse) {
    return throwError(error);
}

  ValidPasswordToken(body): Observable<any> {
    return this.http.post(`${this.baseUrl}valid-password-token`, body);
  }
}