import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { PaymentService } from 'src/app/_services/payment.service';
import { UserService } from 'src/app/_services/user.service';
import { PaymentFormComponent } from '../paymentForm/paymentForm.component';

@Component({
  selector: 'app-paymentsAdministration',
  templateUrl: './paymentsAdministration.component.html',
  styleUrls: ['./paymentsAdministration.component.less']
})
export class PaymentsAdministrationComponent implements OnInit {

  users: any[];
   totalNotGeneratedMedia: any[] = [];
  totalBookedMedia: any[];
  totalWaitingForUserMedia: any[] = [];
  totalWaitingForBookMedia: any[] = [];
  filteredHouseDevelopments: any[] = [];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];
  usersToSendData: any[];
  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();

  constructor(private userService: UserService, 
              private alertifyService: AlertifyService,
              private authService: AuthService,
              private paymentService: PaymentService, 
              public dialog: MatDialog
              ) { }

  ngOnInit() {
    this.getAllusers();
  }

  displayedColumns: string[] = ['Building', 'Payment'];
  displayedPaymentColumns: string[] = ['Name', 'Value', 'Status', 'Action'];

  getAllusers() {
    this.users = [];
    this.filteredHouseDevelopments = [];

    this.userService.getAllusers().subscribe(data => {
      this.users = data as any[];

      this.users.forEach(user => {
        this.paymentService.getPaymentsForUser(user.userId).subscribe(paymentData => {
          user.paymentList = paymentData;
          user.paymentList.forEach(payment => {
            if (payment.paymentStatus == 1)
            payment.paymentStatus = "Czeka na użykownika";
            else if (payment.paymentStatus == 2)
            payment.paymentStatus = "Płatność rozpoczęta";
            else if (payment.paymentStatus == 3)
            payment.paymentStatus = "Płatność przerwana";
            else if (payment.paymentStatus == 4)
            payment.paymentStatus = "Płatność zakończona";
            else if (payment.paymentStatus == 5)
            payment.paymentStatus = "Płatność zaksiegowana";
          });
          console.log(this.users);
        }

        );
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

      this.totalBookedMedia = [];
      this.totalNotGeneratedMedia = [];
      this.totalWaitingForBookMedia = [];
      this.totalWaitingForUserMedia = [];

      this.users.forEach(p => {
        this.paymentService.getPaymentsForUser(p.userId).subscribe(data => {
          console.log(data);
          if ((data as any[]).length === 0) this.totalNotGeneratedMedia.push(data);
          if ((data as any[]).length > 0) {
            (data as any[]).forEach(element => {
              if (element.status === 0) this.totalWaitingForUserMedia.push(data);
              if (element.status === 1) this.totalWaitingForBookMedia.push(data);
              if (element.status === 2) this.totalBookedMedia.push(data);
            });
          }

        })
      });
      console.log(this.users);
      console.log(this.filteredHouseDevelopments);
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
          paymentList: flat.paymentList
        };
        console.log(flat);
        return flat1;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.building === value.building && t.local === value.local
      )));
  }

  openDialog(flat: any){
    const dialogRef = this.dialog.open(PaymentFormComponent, {
      width: '80%',
      data: flat
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
  }

    
    

    unlockPayment(payment: any){

    }
  }
