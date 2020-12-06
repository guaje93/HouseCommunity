import { Component, Inject, OnInit } from '@angular/core';
import { ImagePreviewComponent } from '../ImagePreview/ImagePreview.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-paymentDetails',
  templateUrl: './paymentDetails.component.html',
  styleUrls: ['./paymentDetails.component.scss']
})
export class PaymentDetailsComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<PaymentDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    console.log(this.data);
  }
}
