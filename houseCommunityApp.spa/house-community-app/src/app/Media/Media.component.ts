import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { newPipeline, AnonymousCredential, BlobServiceClient } from '@azure/storage-blob';
import { environment } from 'src/environments/environment';
import * as CryptoJS from 'crypto-js';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { DatePipe } from '@angular/common';
import { MediaService } from '../_services/media.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { FileHelper } from '../Model/fileHelper';

@Component({
  selector: 'app-Media',
  templateUrl: './Media.component.html',
  styleUrls: ['./Media.component.scss']
})
export class MediaComponent implements OnInit {

  constructor(private authService: AuthService,
    private alertifyService: AlertifyService,
    private datePipe: DatePipe,
    private mediaService: MediaService,
    private sanitizer: DomSanitizer
  ) { }

  imageToShow: SafeUrl[];
  isImageLoading: boolean;
  currentFile: File;
  files: FileHelper[] = [];
  descriptions: string[] = [];

  @ViewChild('fileDropRef', { static: false }) fileDropEl: ElementRef;
  ngOnInit(): void {
    this.displayImage();

  }

  onFileDropped($event): void {
    this.prepareFilesList($event);
  }

  updateFiles(): void {

    if (this.files.every(p => p.type != null)) {

      for (const item of this.files) {
        console.log(item);
        this.onFileChange(item);
      }
    }
    else
    {
this.alertifyService.error('Nie wszystkie typy zdjęć zostały zdefiniowane!');
    }
  }

  fileBrowseHandler(files): void {
    this.prepareFilesList(files);
  }

  deleteFile(index: number): void {
    if (this.files[index].file.progress < 100) {
      console.log(this.files[index].file.progress);
      console.log('Upload in progress.');
      return;
    }
    this.files.splice(index, 1);
  }

  displayImage(): void {
    this.imageToShow = [];
    this.mediaService.getMediaForUser(this.authService.decodedToken.nameid).subscribe(
      data => {
        console.log(data);
        const model: any = data;
        model.imageUrl.forEach(element => {
          console.log(element);
          this.mediaService.displayBlobImage(element).subscribe(
            blob => {
              this.createImageFromBlob(blob);
              this.isImageLoading = false;
            }, error => {
              this.isImageLoading = false;
              console.log(error);
            }
          );

        });
      });
  }

  createImageFromBlob(image: Blob): void {
    const reader = new FileReader();
    const blob = new Blob([image], { type: 'application/octet-stream' });
    reader.addEventListener('load', () => {
      this.imageToShow.push(this.sanitizer.bypassSecurityTrustUrl(window.URL.createObjectURL(blob)));
    }, false);
    if (image) {
      reader.readAsDataURL(image);
    }
  }


  async onFileChange(file): Promise<void> {
    console.log(file);
    const currentFileDate = new Date();
    const fileDate = this.datePipe.transform(currentFileDate, 'yyyyMMddHHmmss');

    const newName = this.authService.decodedToken.nameid + '_' + fileDate + '_' + file.file.name;
    this.currentFile = new File([file.file], newName);
    console.log(this.currentFile.name);
    console.log(this.currentFile.type);
    // generate account sas token
    const accountName = environment.accountName;
    const key = environment.key;
    const start = new Date(new Date().getTime() - (15 * 60 * 1000));
    const end = new Date(new Date().getTime() + (30 * 60 * 1000));
    const signedpermissions = 'rwdlac';
    const signedservice = 'b';
    const signedresourcetype = 'sco';
    const signedexpiry = end.toISOString().substring(0, end.toISOString().lastIndexOf('.')) + 'Z';
    const signedProtocol = 'https';
    const signedversion = '2018-03-28';

    const StringToSign =
      accountName + '\n' +
      signedpermissions + '\n' +
      signedservice + '\n' +
      signedresourcetype + '\n' +
      '\n' +
      signedexpiry + '\n' +
      '\n' +
      signedProtocol + '\n' +
      signedversion + '\n';

    const str = CryptoJS.HmacSHA256(StringToSign, CryptoJS.enc.Base64.parse(key));
    const sig = CryptoJS.enc.Base64.stringify(str);


    const sasToken = `sv=${(signedversion)}&ss=${(signedservice)}&srt=${(signedresourcetype)}&sp=${(signedpermissions)}&se=${encodeURIComponent(signedexpiry)}&spr=${(signedProtocol)}&sig=${encodeURIComponent(sig)}`;
    const containerName = environment.containerName;

    const pipeline = newPipeline(new AnonymousCredential(), {
      retryOptions: { maxTries: 4 }, // Retry options
      userAgentOptions: { userAgentPrefix: 'AdvancedSample V1.0.0' }, // Customized telemetry string
      keepAliveOptions: {
        // Keep alive is enabled by default, disable keep alive by setting false
        enable: false
      }
    });

    const blobServiceClient = new BlobServiceClient(`https://${accountName}.blob.core.windows.net?${sasToken}`,
      pipeline);
    const containerClient = blobServiceClient.getContainerClient(containerName);
    if (!containerClient.exists()) {
      console.log('the container does not exit');
      await containerClient.create();

    }
    const client = containerClient.getBlockBlobClient(this.currentFile.name);
    const response = await client.uploadBrowserData(this.currentFile, {
      blockSize: 4 * 1024 * 1024, // 4MB block size
      concurrency: 20, // 20 concurrency
      onProgress: (ev) => console.log(ev),
      blobHTTPHeaders: { blobContentType: this.currentFile.type }
    });
    console.log(containerClient.url);
    console.log(response._response.status);
    if (response._response.status === 201) {
      const req: any = {};
      req.UserId = this.authService.decodedToken.nameid;
      req.ImageUrl = `https://${accountName}.blob.core.windows.net/${containerName}/${this.currentFile.name}`;
      req.UserDescription = file.description;
      req.MediaType = file.type;
      console.log(req);
      this.mediaService.addMediaForUser(req).subscribe(data => {
        console.log(data);
        this.displayImage();
        this.files = [];
      }
      );
      this.alertifyService.success('Zdjęcie zostało przesłane!');
    }
  }

  prepareFilesList(files: Array<any>): void {
    for (const item of files) {
      item.progress = 0;
      console.log((item as File).type);
      if ((item as File).type === 'image/jpeg') {
        const fileHelper = new FileHelper();
        fileHelper.file = item;
        this.files.push(fileHelper);
        this.uploadFilesSimulator(0);
      }
      else {
        this.alertifyService.warning('Nieprawidłowy format pliku. \nZapisz zdjęcie w formacie jpg.');
      }
    }
    this.fileDropEl.nativeElement.value = '';

  }

  formatBytes(bytes, decimals = 2): string {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }


  uploadFilesSimulator(index: number): void {
    setTimeout(() => {
      if (index === this.files.length) {
        return;
      } else {
        const progressInterval = setInterval(() => {
          if (this.files[index].file.progress === 100) {
            clearInterval(progressInterval);
            this.uploadFilesSimulator(index + 1);
          } else {
            this.files[index].file.progress += 10;
          }
        }, 200);
      }
    }, 1000);
  }
}
