import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { AlertifyService } from '../../_services/alertify.service';
import { DatePipe } from '@angular/common';
import { MediaService } from '../../_services/media.service';
import { DomSanitizer } from '@angular/platform-browser';
import { FileHelper } from '../../Model/fileHelper';
import { SingleMediaItem } from '../../Model/SingleMediaItem';
import { BlobService } from '../../_services/blob.service';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-Media',
  templateUrl: './Media.component.html',
  styleUrls: ['./Media.component.scss']
})


export class MediaComponent implements OnInit {

  mediaDataToAdd: FileHelper;
  isImageLoading: boolean;
  files: FileHelper[] = [];
  currentFile: File;
  columnToSort: string;
  textToFilter: string;
  imageToShow: MatTableDataSource<SingleMediaItem>;
  mediaHistory: SingleMediaItem[] = [];
  constructor(private authService: AuthService,
    private alertifyService: AlertifyService,
    private datePipe: DatePipe,
    private mediaService: MediaService,
    private sanitizer: DomSanitizer,
    private blobService: BlobService) {
    this.mediaDataToAdd = new FileHelper();
    this.imageToShow = new MatTableDataSource<SingleMediaItem>();
  }

  @ViewChild('fileDropRef', { static: false }) fileDropEl: ElementRef;
  ngOnInit(): void {
    this.displayImage();
  }

  onFileDropped($event): void {
    this.prepareFilesList($event);
  }

  fileBrowseHandler(files): void {
    this.prepareFilesList(files);
  }

  addManualData(): void {
    if (this.mediaDataToAdd.currentValue && this.mediaDataToAdd.type) {
      const newData = new FileHelper();
      newData.currentValue = this.mediaDataToAdd.currentValue;
      newData.type = this.mediaDataToAdd.type;
      newData.description = this.mediaDataToAdd.description;
      this.files.push(newData);
      this.mediaDataToAdd = new FileHelper();
    }
    else {
      this.alertifyService.error('Uzupełnij brakujące dane!');
    }
  }

  updateFiles(): void {

    if (this.files.some(p => !p.file && !p.currentValue)) {
      this.alertifyService.error('Nie wszystkie wartości zostały zdefiniowane!');
      return;
    }

    if (this.files.every(p => p.type != null)) {
      for (const item of this.files) {
        console.log(item);
        this.onFileChange(item);
      }
    }
    else {
      this.alertifyService.error('Nie wszystkie typy zdjęć zostały zdefiniowane!');
    }
  }


  deleteFile(index: number): void {
    this.files.splice(index, 1);
  }

  displayImage(): void {
    this.mediaHistory = [];
    this.mediaService.getMediaForUser(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        const model: any = data;
        model.singleMediaItems.forEach(element => {
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
        });
      });
  }
  getMediaTypeFromNumber(value: number): string {
    if (value === 0) {
      return 'Woda zimna';
    }
    else {
      return 'Woda ciepła';
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


  async onFileChange(file): Promise<void> {
    console.log(file);

    const currentFileDate = new Date();
    const fileDate = this.datePipe.transform(currentFileDate, 'yyyyMMddHHmmss');

    if (file.file) {

      const newName = this.authService.decodedToken.nameid + '_' + fileDate + '_' + file.file.name;
      this.currentFile = new File([file.file], newName);

      const response = await this.blobService.uploadFile(this.currentFile);

      if (response._response.status === 201) {
        const fileName = this.currentFile.name;
        const id = this.authService.decodedToken.nameid;
        const req: any = this.blobService.createRequestForAddingFile(file, fileName, id);

        this.mediaService.addMediaForUser(req).subscribe(data => {
          console.log(data);
          this.displayImage();
          this.files = [];
          this.alertifyService.success('Zdjęcie zostało przesłane!');
        }
        );
      }
    }
    else {
      const req: any = this.createRequestFromFileItem(file);
      this.mediaService.addMediaForUser(req).subscribe(() => {
        this.displayImage();
        this.files = [];
        this.alertifyService.success('Formularz został wysłany!');
      }
      );
    }
  }

  private createRequestFromFileItem(file: any): any {
    const req: any = {};
    req.UserId = this.authService.decodedToken.nameid;
    req.UserDescription = file.description;
    req.MediaType = file.type;
    req.CurrentValue = file.currentValue;
    return req;
  }

  prepareFilesList(files: Array<any>): void {
    for (const item of files) {
      item.progress = 0;
      console.log((item as File).type);
      if ((item as File).type === 'image/jpeg') {
        const fileHelper = new FileHelper();
        fileHelper.file = item;
        this.files.push(fileHelper);
      }
      else {
        this.alertifyService.warning('Nieprawidłowy format pliku. \nZapisz zdjęcie w formacie jpg.');
      }
    }
    this.fileDropEl.nativeElement.value = '';

  }

}
