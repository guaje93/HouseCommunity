import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { PaymentService } from '../_services/payment.service';

@Component({
  selector: 'app-rent',
  templateUrl: './rent.component.html',
  styleUrls: ['./rent.component.scss']
})
export class RentComponent implements OnInit {

  displayedColumns: string[] = ['Name', 'Value', 'Date', 'Details', 'Payment'];
  payments: any;

  constructor(public paymentService: PaymentService, alertifyService: AlertifyService, public authService: AuthService) { }

  ngOnInit() {
    this.paymentService.getPaymentsForUser(this.authService.decodedToken.nameid).subscribe(
      data => {this.payments = data;
        console.log(data);
      }
      );
    }

    pay(id: number){
this.paymentService.getPaymentUrl(id).subscribe(data => {
  console.log(data);
  let result: any = data;
  window.open(result.path, "blank");
})
    }
    
  


}
