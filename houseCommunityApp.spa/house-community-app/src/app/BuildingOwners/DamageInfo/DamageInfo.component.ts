import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ImagePreviewComponent } from 'src/app/residents/ImagePreview/ImagePreview.component';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BlobService } from 'src/app/_services/blob.service';

@Component({
  selector: 'app-DamageInfo',
  templateUrl: './DamageInfo.component.html',
  styleUrls: ['./DamageInfo.component.scss']
})
export class DamageInfoComponent implements OnInit {

  constructor(  public dialogRef: MatDialogRef<DamageInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private blobService: BlobService,
    private alertifyService: AlertifyService,
    private datePipe: DatePipe,
    private authService: AuthService,
    private dialog: MatDialog) { }

  ngOnInit() {
    console.log(this.data);
  }

  openImagePreview(imageUrl: string): void {
    const dialogRef = this.dialog.open(ImagePreviewComponent, {
      width: '80%',
      data: imageUrl
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }

}
