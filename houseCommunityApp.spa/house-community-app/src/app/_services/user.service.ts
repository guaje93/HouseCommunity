import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = "http://localhost:5000/api/user/";

  constructor(private http: HttpClient) {}

  getUser(id: number) {
    return this.http.get(this.baseUrl + id).pipe(
     retry(3)
    );
  }
}
