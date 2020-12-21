import { Component, OnInit } from '@angular/core';
import { FileTypeEnum } from 'src/app/Model/fileTypeEnum';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BlobService } from 'src/app/_services/blob.service';
import { DamageService } from 'src/app/_services/damage.service';

@Component({
  selector: 'app-Damage',
  templateUrl: './Damage.component.html',
  styleUrls: ['./Damage.component.scss']
})
export class DamageComponent implements OnInit {

  files = [];
  data: any = {};
  constructor(private blobService: BlobService, private alertifyService: AlertifyService, private authService: AuthService, private damageService: DamageService) { }

  ngOnInit() {
  }
  onFileDropped($event): void {
    console.log($event);
    this.prepareFilesList($event);
  }

  fileBrowseHandler(files): void {
    console.log(files);
    this.prepareFilesList(files);
  }
  deleteFile(index: number): void {
    this.files.splice(index, 1);
  }

  prepareFilesList(files: Array<File>): void {
    for (const item of files) {
      this.files.push(item);

    }
  }
  addDamage() {

    if (!this.data.title || !this.data.description) {
      this.alertifyService.error('Wypełnij wymagane pola!')
      return;
    }
    let filesToSend = this.files;
    const id = this.authService.decodedToken.nameid;
    const req: any = {};
    req.title = this.data.title;
    req.description = this.data.description;
    req.userId = this.authService.decodedToken.nameid;

    this.damageService.addDamage(req).subscribe(data => {
      console.log(data);

      if (this.files?.length > 0) {

        filesToSend.forEach(async file => {
          const response = await this.blobService.uploadFile(file, FileTypeEnum.DAMAGE);
          if (response._response.status === 201) {
            let model: any = {};
            model.id = (data as any).id;
            model.fileName = file.name;
            model.fileUrl = this.blobService.getUrl(file.name)
            this.damageService.updateImageInfo(model).subscribe(image => {
              this.alertifyService.success('Zdjęcie zostało zapisane');
            });
          }
        });
      }
      this.files = [];
      this.data = {};
      this.alertifyService.success('Zgłoszenie zostało dodane');

    });
  }
}
