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

createNewPayment(paymentData: any) {
  return this.http.post(this.baseUrl + 'create-new-payment/', paymentData);
}

createNewCustomPayment(paymentData: any) {
  return this.http.post(this.baseUrl + 'create-new-custom-payment/', paymentData);
}

getPaymentDetails(element: Date, id: number) {
  return this.http.post(this.baseUrl + 'calculate-costs/' + id, element);
}
bookPayment(model: any) {
  return this.http.post(this.baseUrl + 'book-payment/', model);
}
unlockPayment(model: any) {
  return this.http.post(this.baseUrl + 'unlock-payment/', model);
}
removePayment(model: any) {
  return this.http.post(this.baseUrl + 'remove-payment/', model);
}


}
