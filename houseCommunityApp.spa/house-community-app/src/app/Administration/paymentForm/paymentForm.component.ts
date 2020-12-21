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
        this.paymentData.userId = this.authService
this.paymentData = this.authService.decodedToken.nameid;
this.paymentData.flatId = this.data.flatId;
this.paymentData.period = new Date();
      }
      );
  }

  addPayment(){
    this.paymentService.createNewPayment(this.paymentData).subscribe(data => {
      this.alertifyService.success('Payment added');
    this.dialogRef.close();

    })
  }
}
