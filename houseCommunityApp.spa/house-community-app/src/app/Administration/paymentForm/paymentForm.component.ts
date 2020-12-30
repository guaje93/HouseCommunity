import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImagePreviewComponent } from 'src/app/residents/ImagePreview/ImagePreview.component';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PaymentService } from 'src/app/_services/payment.service';

@Component({
  selector: 'app-paymentForm',
  templateUrl: './paymentForm.component.html',
  styleUrls: ['./paymentForm.component.scss']
})
export class PaymentFormComponent implements OnInit {

  paymentData:any;
  constructor(
    public dialogRef: MatDialogRef<ImagePreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, 
    private paymentService: PaymentService, 
    private alertifyService: AlertifyService,
    private authService: AuthService) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    this.paymentService.getPaymentDetails(new Date(), this.data.flatId).subscribe(
      data => 
      {console.log(data);
        this.paymentData = data;
        this.paymentData.userId = this.authService.decodedToken.nameid;
this.paymentData.flatId = this.data.flatId;
this.paymentData.period = this.GetDate(this.data.period);
      }
      );
  }

  addPayment(){
    
    if(this.paymentData.deadline){
      this.paymentService.createNewPayment(this.paymentData).subscribe(data => {
      this.alertifyService.success('Płatność dodana');
    this.dialogRef.close();
      });
    }
    else{
      this.alertifyService.error('Podaj wszystkie dane');
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
