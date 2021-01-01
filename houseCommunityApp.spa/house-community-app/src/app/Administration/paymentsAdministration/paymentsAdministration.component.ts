import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PaymentService } from 'src/app/_services/payment.service';
import { UserService } from 'src/app/_services/user.service';
import { CustomPaymentComponent } from '../customPayment/customPayment.component';
import { PaymentFormComponent } from '../paymentForm/paymentForm.component';
import {MatSnackBar} from '@angular/material/snack-bar';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { UnlockPaymentFormComponent } from '../UnlockPaymentForm/UnlockPaymentForm.component';

@Component({
  selector: 'app-paymentsAdministration',
  templateUrl: './paymentsAdministration.component.html',
  styleUrls: ['./paymentsAdministration.component.less']
})
export class PaymentsAdministrationComponent implements OnInit {

  users: any[];

  filteredHouseDevelopments: any[] = [];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredPeriods: any[];
  filteredUsers: any[];
  usersToSendData: any[];
  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  periodForm = new FormControl();

  constructor(private userService: UserService,
    private alertifyService: AlertifyService,
    private authService: AuthService,
    private paymentService: PaymentService,
    public dialog: MatDialog,
    private _bottomSheet: MatBottomSheet
  ) { }

  ngOnInit() {
    this.getAllusers();
  }

  displayedColumns: string[] = ['Building', 'Payment'];
  displayedPaymentColumns: string[] = ['Name', 'Value', 'Status', 'Action'];

  getAllusers() {

    this.users = [];
    this.userService.getAllusers().subscribe(data => {
      this.users = data as any[];

      this.users.forEach(user => {
        this.paymentService.getPaymentsForUser(user.flatId).subscribe(paymentData => {
          user.paymentList = paymentData;
          this.showFlatsList();
        });
      })
      this.filteredHouseDevelopments = this.users.map(user => {
        let houseDev =
        {
          housingDevelopmentName: user.housingDevelopmentName,
          housingDevelopmentId: user.housingDevelopmentId
        };
        return houseDev;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.housingDevelopmentId === value.housingDevelopmentId && t.housingDevelopmentName === value.housingDevelopmentName
      )));
    });

  }


  filterBuildings($event) {
    this.filteredFlats = [];
    this.filteredUsers = [];

    this.filteredBuildings = this.users
      .filter(user => $event.value.includes(user.housingDevelopmentId))
      .map(user => {
        let building =
        {
          buildingId: user.buildingId,
          address: user.address
        };
        return building;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.buildingId === value.buildingId && t.address === value.address
      )));
  }

  filterPeriods($event) {
    this.filteredFlats = [];
    this.filteredUsers = [];
    let periods = [];
    this.users
      .filter(user => $event.value.includes(user.buildingId))
      .map(user => {
        user.paymentList.forEach(payment => {
          if (!periods.includes('M' + payment.month + 'Y' + payment.year))
            periods.push('M' + payment.month + 'Y' + payment.year);

        });
      })

    if (!periods.includes(this.getCurrentPeriod()))
      periods.push(this.getCurrentPeriod());
    this.filteredPeriods = periods.sort();
    this.periodForm.reset();
    this.usersToSendData = [];

  }

  getCurrentPeriod() {
    let year = new Date().getFullYear();
    let month = new Date().getMonth() + 1;
    let period = '';
    period = "M" + month + "Y" + year;
    console.log(period);
    return period;
  }

  showFlatsList() {
    this.usersToSendData = this.users
      .filter(flat => this.buildingsFrom.value.includes(flat.buildingId))
      .map(flat => {
        let list: any = {};

        let flat1 =
        {
          flatId: flat.flatId,
          building: flat.address,
          local: flat.localNumber,
          paymentList: flat.paymentList.filter(p => p.period == this.periodForm.value)
        };
        console.log(flat);
        return flat1;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.building === value.building && t.local === value.local
      ))).sort(
        function (a, b) {
          if (a.building === b.building) {
            return a.local - b.local;
          }
          return a.building > b.building ? 1 : -1;
        }
      );
  }

  openDialog(flat: any) {
    flat.period = this.periodForm.value
    console.log(flat);
    const dialogRef = this.dialog.open(PaymentFormComponent, {
      width: '80%',
      data: flat
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
      this.getAllusers();

    });
  }

  openCustomPaymentForm(flat: any) {
    let data: any = {};
    data.flatId = flat;
    data.period = this.periodForm.value

    const dialogRef = this.dialog.open(CustomPaymentComponent, {
      width: '80%',
      data: data
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
      this.getAllusers();

    });
  }

  public decodePeriod(encodedPeriod: string) {

    let year = encodedPeriod.substring(encodedPeriod.indexOf('Y') + 1);
    let month = encodedPeriod.substring(1, encodedPeriod.indexOf('Y'));

    switch (month) {
      case "1": month = "Styczeń"; break;
      case "2": month = "Luty"; break;
      case "3": month = "Marzec"; break;
      case "4": month = "Kwiecień"; break;
      case "5": month = "Maj"; break;
      case "6": month = "Czerwiec"; break;
      case "7": month = "Lipiec"; break;
      case "8": month = "Sierpień"; break;
      case "9": month = "Wrzesień"; break;
      case "10": month = "Październik"; break;
      case "11": month = "Listopad"; break;
      case "12": month = "Grudzień"; break;
    }

    return month + ' ' + year;
  }

  filterRentItems(items) {
    return items.filter(x => x.type === 1);
  }


  bookPayment(payment: any) {
    let model: any = {};
    model.paymentId = payment.id;
    model.userId = this.authService.decodedToken.nameid;
    this.paymentService.bookPayment(model).subscribe(
      data => {
        this.alertifyService.success('Zaksięgowano płatność');
        this.getAllusers();
      }
    )
  }

  unlockPayment(payment: any) {
    this._bottomSheet._openedBottomSheetRef = this._bottomSheet.open(UnlockPaymentFormComponent, {
      data: payment
    });
    this._bottomSheet._openedBottomSheetRef.afterDismissed().subscribe(() => {
      this.getAllusers();
    });
  }

  decodePaymentStatus(status: number) {
    if (status == 1)
      return "Czeka na użytkownika";
    else if (status == 2)
      return "Płatność rozpoczęta";
    else if (status == 3)
      return "Płatność przerwana";
    else if (status == 4)
      return "Płatność zakończona";
    else if (status == 5)
      return "Płatność zaksiegowana";
  }


  removePayment(payment: any) {
    let model: any = {};
    model.paymentId = payment.id;
    model.userId = this.authService.decodedToken.nameid;
    

    this.paymentService.removePayment(model).subscribe(
      data => {
        this.alertifyService.success('Usunięto płatność');
        this.getAllusers();
      }
    )
  }
}
