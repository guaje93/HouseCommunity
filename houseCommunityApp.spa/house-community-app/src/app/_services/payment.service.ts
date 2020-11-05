import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

baseUrl = 'http://localhost:5000/api/payment/';
constructor(private http: HttpClient) {}

getPaymentsForUser(id: number) {
  return this.http.get(this.baseUrl + id);
}

getPaymentUrl(id: number) {
  return this.http.get(this.baseUrl + 'pay/' + id);
}
}
