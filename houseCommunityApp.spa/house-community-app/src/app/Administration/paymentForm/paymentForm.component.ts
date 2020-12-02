import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImagePreviewComponent } from 'src/app/residents/ImagePreview/ImagePreview.component';

@Component({
  selector: 'app-paymentForm',
  templateUrl: './paymentForm.component.html',
  styleUrls: ['./paymentForm.component.scss']
})
export class PaymentFormComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ImagePreviewComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    console.log(this.data);
  }
}
