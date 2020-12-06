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

createNewOrder(element: any) {
  return this.http.post(this.baseUrl + 'create-new-order',element);
}

createNewPayment(element: Date, id: number) {
  return this.http.post(this.baseUrl + 'calculate-costs/' + id, element);
}
}
