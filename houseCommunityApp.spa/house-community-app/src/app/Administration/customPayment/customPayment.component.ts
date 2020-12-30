import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-customPayment',
  templateUrl: './customPayment.component.html',
  styleUrls: ['./customPayment.component.less']
})
export class CustomPaymentComponent implements OnInit {

  public paymentData: any = {};
  constructor(
    public dialogRef: MatDialogRef<CustomPaymentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    @Inject(MAT_DIALOG_DATA) public period: any, 
    private alertifyService: AlertifyService,
    private paymentService: PaymentService,
    private authService: AuthService
  ) { 

    console.log(this.data);

  }

  ngOnInit() {
    this.paymentData.flatId = this.data.flatId;
    this.paymentData.name = '';
    this.paymentData.description = '';
    this.paymentData.value = 0;
    this.paymentData.userId = this.authService.decodedToken.nameid;
    this.paymentData.period = this.GetDate(this.data.period);
  }

  addPayment(){
if(this.paymentData.name && this.paymentData.description && this.paymentData.value && this.paymentData.deadline){
this.paymentService.createNewCustomPayment(this.paymentData).subscribe(data => {
  console.log(data);
this.alertifyService.success('Płatność dodana');
this.dialogRef.close();
})
}
else{
  console.log(this.paymentData)
this.alertifyService.error('Podaj wszystkie niezbędne dane')
}
  }

  GetDate(period: string): Date{
    let year = +period.substring(period.indexOf('Y')+1) ;
    let month = +period.substring(1, period.indexOf('Y')) ;
    console.log(year);
    console.log(month);

    let date = new Date(year, month-1, 15);
    console.log(date);
    return date;
  }
}
