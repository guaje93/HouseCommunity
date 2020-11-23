import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MediaToUpdate } from 'src/app/Model/SingleMediaItem';
import { BlobService } from 'src/app/_services/blob.service';

@Component({
  selector: 'app-AddMediaForm',
  templateUrl: './AddMediaForm.component.html',
  styleUrls: ['./AddMediaForm.component.scss']
})
export class AddMediaFormComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<AddMediaFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: MediaToUpdate,
    private blobService: BlobService) {}

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

  saveData(){

  }
}
