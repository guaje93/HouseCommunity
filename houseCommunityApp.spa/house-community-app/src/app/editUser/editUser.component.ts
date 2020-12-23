import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FileTypeEnum } from '../Model/fileTypeEnum';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { BlobService } from '../_services/blob.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-editUser',
  templateUrl: './editUser.component.html',
  styleUrls: ['./editUser.component.scss']
})
export class EditUserComponent implements OnInit {

  phoneDisabled: boolean;
  residentsAmountDisabled: boolean;
  emailDisabled: boolean;
  hotWaterUsageDisabled: boolean;
  coldWaterUsageDisabled: boolean;
  heatingUsageDisabled: boolean;
  phoneNumber: string;
  residentsAmount: number;
  area: number;
  hotWaterEstimatedUsage: number;
  coldWaterEstimatedUsage: number;
  heatingEstimatedUsage: number;
  firstName: string;
  lastName: string;
  email: string;
  birthDate: Date;
  hotWaterDescription: string;
  coldWaterDescription: string;
  heatingDescription: string;
  file: File;
  fileUrl: string;
  userRole: number;
  private fileReader = new FileReader();
  public base64File: string;

  constructor(
    public dialogRef: MatDialogRef<EditUserComponent>,
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private userService: UserService,
    private blobService: BlobService) {

  }

  ngOnInit() {
    let user: any;
    this.userService.getUser(this.authService.decodedToken.nameid).subscribe(data => {
      user = data;
      console.log(data);
      this.phoneDisabled = true;
      this.emailDisabled = true;
      this.firstName = user.firstName;
      this.lastName = user.lastName;
      this.phoneNumber = user.phoneNumber;
      this.email = user.email;
      this.area = user.area;
      this.userRole = user.userRole;

      if (user.userRole === 1) {
        this.residentsAmountDisabled = true;
        this.heatingUsageDisabled = true;
        this.hotWaterUsageDisabled = true;
        this.coldWaterUsageDisabled = true;
        this.residentsAmount = user.residentsAmount;
        this.hotWaterEstimatedUsage = user.hotWaterEstimatedUsage;
        this.coldWaterEstimatedUsage = user.coldWaterEstimatedUsage;
        this.heatingEstimatedUsage = user.heatingEstimatedUsage;
        this.hotWaterDescription = "Cena jednostkowa za 1m3 wody ciepłej: " + user.hotWaterUnitCost + "zł";
        this.coldWaterDescription = "Cena jednostkowa za 1m3 wody zimnej: " + user.coldWaterUnitCost + "zł";
        this.heatingDescription = "Cena jednostkowa za 1GJ energii na ogrzewanie: " + user.heatingUnitCost + "zł";
      }
      this.fileUrl = user.avatarUrl;
    });
  }

  onFileDropped($event): void {
    if ($event[0].type == 'image/jpeg') {
      this.file = $event[0];
      this.readFiles(this.file);
    }
    else {
      this.alertifyService.warning("Zły format pliku. Wybierz format jpeg")
    }
  }

  private readFiles(file: any) {
    this.fileReader.onload = () => {
      this.base64File = this.fileReader.result as string;
    };
    this.fileReader.readAsDataURL(file);
  }

  private async addFile() {
    if (this.file) {
      const currentFileDate = new Date();
      console.log(this.file);
      const newName = this.authService.decodedToken.nameid + '_' + this.file.name;
      let currentFile = new File([this.file], newName);
      let response = await this.blobService.uploadFile(currentFile, FileTypeEnum.AVATAR);

      if (response._response.status === 201) {
        const fileName = currentFile.name;
        this.fileUrl = this.blobService.getUrl(newName);
      }
    }
  }

  fileBrowseHandler($event): void {

    console.log($event);
    if ($event[0].type == 'image/jpeg') {

      this.file = $event[0];
      this.readFiles(this.file);
    }
    else {
      this.alertifyService.warning("Zły format pliku. Wybierz format jpeg")
    }
  }

  deleteFile() {
    this.fileUrl = this.base64File = '';
    this.file = null;
  }
  public async saveData() {
    let model: any = {};
    model.id = this.authService.decodedToken.nameid;
    model.email = this.email;
    model.phoneNumber = this.phoneNumber;
    model.residentsAmount = this.residentsAmount;
    model.hotWaterEstimatedUsage = this.hotWaterEstimatedUsage;
    model.coldWaterEstimatedUsage = this.coldWaterEstimatedUsage;
    model.heatingEstimatedUsage = this.heatingEstimatedUsage;
    await this.addFile();
    if (this.file)
      model.avatarUrl = this.fileUrl;
    this.userService.updateUserContactData(model).subscribe(
      data => {
        this.alertifyService.success("Zapisano zmienione dane")
        this.residentsAmountDisabled = this.phoneDisabled = this.emailDisabled = this.coldWaterUsageDisabled = this.hotWaterUsageDisabled = this.heatingUsageDisabled = true;
      this.dialogRef.close();
      }),
      error => {
        this.alertifyService.error("Wystąpił bląd. Dane nie zostały zapisane.")
      };



  }
}
