import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
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
              public dialog: MatDialog
              ) { }

  ngOnInit() {
    this.getAllusers();
  }

  displayedColumns: string[] = ['Building', 'Generate'];
  displayedMediaColumns: string[] = ['Type', 'StartDate', 'EndDate', 'Value', 'Accept'];

  getAllusers() {
    this.users = [];
    this.filteredHouseDevelopments = [];

    this.userService.getAllusers().subscribe(data => {
      this.users = data as any[];

     
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
          mediaList: flat.mediaList
        };
        console.log(flat);
        return flat1;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.building === value.building && t.local === value.local
      )));
  }

  generateEmptyPayment(flat: any){
      const dialogRef = this.dialog.open(PaymentFormComponent, {
        width: '80%',
        data: flat
      });
  
      dialogRef.afterClosed().subscribe(() => {
        console.log('The dialog was closed');
      });
    }
  }
