import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PaymentDetailsComponent } from '../paymentDetails/paymentDetails.component';
import { AlertifyService } from '../../_services/alertify.service';
import { AuthService } from '../../_services/auth.service';
import { PaymentService } from '../../_services/payment.service';

@Component({
  selector: 'app-rent',
  templateUrl: './rent.component.html',
  styleUrls: ['./rent.component.scss']
})
export class RentComponent implements OnInit {

  displayedColumns: string[] = ['Name', 'Value', 'Date', 'Details', 'Payment'];
  displayedColumnsForHistory: string[] = ['Name', 'Value', 'PaymentDate', 'BookDate', 'Details', 'PaymentStatus', 'BookStatus'];
  notCompletedPayments: any;
  completedPayments: any;
  paymentsHistory: any;

  constructor(public paymentService: PaymentService, alertifyService: AlertifyService, public authService: AuthService, public dialog: MatDialog) { }

  ngOnInit() {
    this.paymentService.getPaymentsForUser(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        let payments = data as any[];
        this.completedPayments = payments.filter(p => p.paymentStatus >= 4);
        this.notCompletedPayments = payments.filter(p => p.paymentStatus < 4);
      }
    );
  }

  pay(payment: any) {
    let model: any = {};
    model.id = payment.id;
    model.price = payment.value;
    this.paymentService.createNewOrder(model).subscribe(data => {
      console.log(data);
      let result: any = data;
      window.open(result.path, "blank");
    })
  }

  hidePaymentOption(payment: any): boolean {
    if (payment.status == "PENDING" || payment.status == "WAITING_FOR_CONFIRMATION" || payment.status == "COMPLETED")
      return false;
    else
      return true;
  }

  decodePaymentStatus(status: number){
    if(status === 4)
      return "Opłacono";
      else if (status === 5)
      return "Zaksięgowano";
  }


  showDetails(payment: any) {
    const dialogRef = this.dialog.open(PaymentDetailsComponent, {
      width: '40%',
      data: payment
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }
}
