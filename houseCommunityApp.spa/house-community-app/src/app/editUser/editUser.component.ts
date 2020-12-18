import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
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
  constructor(
    public authService: AuthService,
    public alertifyService: AlertifyService,
    public userService: UserService) {

  }

  ngOnInit() {
    let user: any;
    this.userService.getUser(this.authService.decodedToken.nameid).subscribe(data => {
      user = data;
      this.residentsAmountDisabled = true;
      this.phoneDisabled = true;
      this.emailDisabled = true;
      this.coldWaterUsageDisabled = true;
      this.hotWaterUsageDisabled = true;
      this.heatingUsageDisabled = true;
      this.firstName = user.firstName;
      this.lastName = user.lastName;
      this.birthDate = user.birthdate;
      this.phoneNumber = user.phoneNumber;
      this.email = user.email;
      this.area = user.area;
      this.residentsAmount = user.residentsAmount;
      this.hotWaterEstimatedUsage = user.hotWaterEstimatedUsage;
      this.coldWaterEstimatedUsage = user.coldWaterEstimatedUsage;
      this.heatingEstimatedUsage = user.heatingEstimatedUsage;
    this.hotWaterDescription = "Cena jednostkowa za 1m3 wody ciepłej: " + user.hotWaterUnitCost + "zł";
    this.coldWaterDescription = "Cena jednostkowa za 1m3 wody zimnej: " + user.coldWaterUnitCost + "zł";
    this.heatingDescription = "Cena jednostkowa za 1GJ energii na ogrzewanie: " + user.heatingUnitCost + "zł";
  });
  }

  public editResidentsAmount() {
    this.residentsAmountDisabled = !this.residentsAmountDisabled;
  }

  public editEmail() {
    this.emailDisabled = !this.emailDisabled;
  }
  public editPhone() {
    this.phoneDisabled = !this.phoneDisabled;
  }

  public editHeatingUsage() {
    this.heatingUsageDisabled = !this.heatingUsageDisabled;
  }

  public editColdWaterUsage() {
    this.coldWaterUsageDisabled = !this.coldWaterUsageDisabled;
  }

  public editHotWaterUsage() {
    this.hotWaterUsageDisabled = !this.hotWaterUsageDisabled;
  }

  public saveData() {
    let model: any = {};
    model.id = this.authService.decodedToken.nameid;
    model.email = this.email;
    model.phoneNumber = this.phoneNumber;
    model.residentsAmount = this.residentsAmount;
    model.hotWaterEstimatedUsage = this.hotWaterEstimatedUsage;
    model.coldWaterEstimatedUsage = this.coldWaterEstimatedUsage;
    model.heatingEstimatedUsage = this.heatingEstimatedUsage;
    this.userService.updateUserContactData(model).subscribe(
      data => {
        this.alertifyService.success("Zapisano zmienione dane")
        this.residentsAmountDisabled = true;
        this.phoneDisabled = true;
        this.emailDisabled = true;
        this.coldWaterUsageDisabled = true;
        this.hotWaterUsageDisabled = true;
        this.heatingUsageDisabled = true;
            }),
      error => {
        this.alertifyService.error("Wystąpił bląd. Dane nie zostały zapisane.")
      };

  }
}
