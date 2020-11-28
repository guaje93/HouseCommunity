import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { DatePipe } from '@angular/common';
import { MediaService } from '../../_services/media.service';
import { DomSanitizer } from '@angular/platform-browser';
import { FileHelper } from '../../Model/fileHelper';
import { MediaToUpdate, SingleMediaItem } from '../../Model/SingleMediaItem';
import { BlobService } from '../../_services/blob.service';
import { MatTableDataSource } from '@angular/material/table';
import { FileTypeEnum } from 'src/app/Model/fileTypeEnum';
import { Role } from '../../Model/Role';
import { AddMediaFormComponent } from '../AddMediaForm/AddMediaForm.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-Media',
  templateUrl: './Media.component.html',
  styleUrls: ['./Media.component.scss']
})


export class MediaComponent implements OnInit {
  Role = Role;
  mediaDataToAdd: FileHelper;
  isImageLoading: boolean;
  files: FileHelper[] = [];
  currentFile: File;
  columnToSort: string;
  textToFilter: string;
  imageToShow: MatTableDataSource<SingleMediaItem>;
  mediaToUpdate: MatTableDataSource<MediaToUpdate>;

  mediaHistory: SingleMediaItem[] = [];

  constructor(private authService: AuthService,
    private alertifyService: AlertifyService,
    private datePipe: DatePipe,
    private mediaService: MediaService,
    private sanitizer: DomSanitizer,
    private blobService: BlobService,
    public dialog: MatDialog) {
    this.mediaDataToAdd = new FileHelper();
    this.imageToShow = new MatTableDataSource<SingleMediaItem>();
    this.mediaToUpdate = new MatTableDataSource<MediaToUpdate>();
  }
  displayedColumns: string[] = ['MediaType', 'StartDate', 'EndDate', 'LastValue', 'Update'];

  @ViewChild('fileDropRef', { static: false }) fileDropEl: ElementRef;
  ngOnInit(): void {
    this.displayImage();
    this.Initialize();
  }

  private Initialize() {
    this.mediaService.getMediaForUser(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        let arr = [];
        (data as any).singleMediaItems.forEach(element => {
          if (element.status === 0) {

            let media = new MediaToUpdate();
            media.Id = element.id;
            media.MediaType = this.getMediaTypeFromNumber(element.mediaEnum);

            media.StartPeriodDate = element.startPeriodDate;
            media.EndPeriodDate = element.endPeriodDate;
            media.LastValue = element.lastValue;
            arr.push(media);
            console.log(this.mediaToUpdate);
          }
        });
        this.mediaToUpdate.data = arr;
      });
  }

  displayImage(): void {
    this.mediaHistory = [];
    this.mediaService.getMediaForUser(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        const model: any = data;
        model.singleMediaItems.forEach(element => {
          if(element.status === 2){

            console.log(element);
            if (element.imageUrl) {

              this.mediaService.displayBlobImage(element.imageUrl).subscribe(
                blob => {
                  this.createImageFromBlob(blob, element);
                this.isImageLoading = false;
              }, error => {
                this.isImageLoading = false;
                console.log(error);
              }
              );
              
            }
            else {
              const item = new SingleMediaItem();
              item.Description = element.description,
              item.FileName = element.fileName;
              item.CreationDate = element.creationDate;
              item.CurrentValue = element.currentValue;
              item.MediaType = this.getMediaTypeFromNumber(element.mediaEnum);
              this.mediaHistory.push(item);
              this.imageToShow.data = this.mediaHistory;
              
            }
          }
        });
      });
  }
  getMediaTypeFromNumber(value: number): string {
    if (value === 1) {
      return 'Woda zimna';
    }
    else if (value === 2){
      return 'Woda ciepła';
    }
    else if (value === 3){
      return 'Ogrzewanie';
    }
  }
  createImageFromBlob(image: Blob, mediaItem: any): void {
    const reader = new FileReader();
    const blob = new Blob([image], { type: 'application/octet-stream' });
    reader.addEventListener('load', () => {

      const item = new SingleMediaItem();
      item.ImageUrl = this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(blob));
      item.Description = mediaItem.description,
        item.FileName = mediaItem.fileName;
      item.CreationDate = mediaItem.creationDate;
      item.CurrentValue = mediaItem.currentValue;
      item.MediaType = this.getMediaTypeFromNumber(mediaItem.mediaEnum);
      this.mediaHistory.push(item);
      this.imageToShow.data = this.mediaHistory;
      console.log(this.imageToShow);
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
  }


  openDialog(media: MediaToUpdate): void {
    const dialogRef = this.dialog.open(AddMediaFormComponent, {
      width: '80%',
      data: media
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
      this.displayImage();
      this.Initialize();
    });
  }

}
