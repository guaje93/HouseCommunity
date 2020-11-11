import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BlobService } from 'src/app/_services/blob.service';
import { UserService } from 'src/app/_services/user.service';
import { FormControl } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-AnnouncementsAdministration',
  templateUrl: './AnnouncementsAdministration.component.html',
  styleUrls: ['./AnnouncementsAdministration.component.less']
})
export class AnnouncementsAdministrationComponent implements OnInit {

  files: File[] = [];
  currentFile: File;
  users: any[];
  filteredHouseDevelopments: any[];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];

  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  flatsFrom = new FormControl();
  usersFrom = new FormControl();

usersToSendData: any[];

  constructor(public blobService: BlobService, private alertifyService: AlertifyService, private userService: UserService, private cd: ChangeDetectorRef) {
  }

  displayedColumns: string[] = ['Name', 'Email', 'Address'];
  
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

  filterFlats($event) {
    console.log($event);

    this.filteredUsers = [];
    this.filteredFlats = this.users
      .filter(user => $event.value.includes(user.buildingId))
      .map(user => {
        let flat =
        {
          flatId: user.flatId,
          localNumber: user.address + ", m. " + user.localNumber
        };
        return flat;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.localNumber === value.localNumber
      )));
      this.showUsersList();
  }

  filterUsers($event) {
    console.log($event);
    this.filteredUsers = this.users
      .filter(user => $event.value.includes(user.flatId))
      .map(user => {
        let flat =
        {
          userId: user.userId,
          name: user.name
        };
        return flat;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.userId === value.userId && t.name === value.name
      )));
      this.showUsersList();
  }

  showUsersList(){
    console.log(this.usersFrom);
    this.usersToSendData = this.users
      .filter(user => this.usersFrom.value.includes(user.userId))
      .map(user => {
        let flat =
        {
          userId: user.userId,
          name: user.name,
          userEmail: user.userEmail,
          address: user.address + ', m:' + user.localNumber 
        };
        return flat;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.userId === value.userId && t.name === value.name && t.userEmail === value.userEmail && t.address === value.address
      )));
  }


  @ViewChild('fileDropRef', { static: false }) fileDropEl: ElementRef;
  ngOnInit() {
    this.getAllusers();
  }
  onFileDropped($event): void {
    this.prepareFilesList($event);
  }

  fileBrowseHandler(files): void {
    this.prepareFilesList(files);
  }

  prepareFilesList(files: Array<any>): void {
    for (const item of files) {
      item.progress = 0;
      console.log((item as File).type);
      if ((item as File).type === 'application/pdf') {

        this.files.push(item);
      }
      else {
        this.alertifyService.warning('Nieprawidłowy format pliku. \nZapisz zdjęcie w formacie jpg.');
      }
    }
    this.fileDropEl.nativeElement.value = '';
  }
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



      console.log(this.users);
      console.log(this.filteredHouseDevelopments);
    });
  }
}
