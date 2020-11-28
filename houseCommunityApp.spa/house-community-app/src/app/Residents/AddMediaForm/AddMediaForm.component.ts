import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FileTypeEnum } from 'src/app/Model/fileTypeEnum';
import { MediaToUpdate } from 'src/app/Model/SingleMediaItem';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BlobService } from 'src/app/_services/blob.service';
import { MediaService } from 'src/app/_services/media.service';

@Component({
  selector: 'app-AddMediaForm',
  templateUrl: './AddMediaForm.component.html',
  styleUrls: ['./AddMediaForm.component.scss']
})
export class AddMediaFormComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<AddMediaFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MediaToUpdate,
    private blobService: BlobService,
    private mediaService: MediaService,
    private alertifyService: AlertifyService,
    private datePipe: DatePipe,
    private authService: AuthService) { }

  file: File;
  media: any = {};


  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    console.log(this.data);
    this.media.Id = this.data.Id;
  }

  onFileDropped($event): void {
    this.file = $event[0];
  }

  fileBrowseHandler($event): void {
    this.file = $event[0];
  }

  deleteFile(): void {
    this.file = null;
  }

  async saveData() {

    let response: any;
    if (this.file) {
      const currentFileDate = new Date();
      const fileDate = this.datePipe.transform(currentFileDate, 'yyyyMMddHHmmss');
      const newName = this.authService.decodedToken.nameid + '_' + fileDate + '_' + this.file.name;
      let currentFile = new File([this.file], newName);
      response = await this.blobService.uploadFile(currentFile, FileTypeEnum.MEDIA);

      if (response._response.status === 201) {
        const fileName = currentFile.name;

        const req: any = this.blobService.createRequestForAddingFile(this.media.Description, this.media.CurrentValue, fileName, this.media.Id);

        this.mediaService.updateMedia(req).subscribe(data => {
          console.log(data);
          this.file = null
          this.alertifyService.success('Formularz ze zdjęciem został wysłany!');
          this.dialogRef.close();
        }
        );
      }
      else
        this.alertifyService.error('Formularz nie zostła wysłany! Błąd!');
    }
    else {
      const req: any = this.blobService.createRequestForAddingFile(this.media.Description, this.media.CurrentValue, '', this.media.Id);

      this.mediaService.updateMedia(req).subscribe(data => {
        console.log(data);
        this.file = null
        this.alertifyService.success('Formularz został wysłany!');
        this.dialogRef.close();
      });
    }

  }
}
