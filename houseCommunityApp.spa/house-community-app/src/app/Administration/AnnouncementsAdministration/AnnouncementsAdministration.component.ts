import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BlobService } from 'src/app/_services/blob.service';
import { UserService } from 'src/app/_services/user.service';
import { FormControl } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { DatePipe } from '@angular/common';
import { AnnouncementService } from 'src/app/_services/announcement.service';
import { FileTypeEnum } from 'src/app/Model/fileTypeEnum';
import { Announcement } from 'src/app/Model/announcement';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';

@Component({
  selector: 'app-AnnouncementsAdministration',
  templateUrl: './AnnouncementsAdministration.component.html',
  styleUrls: ['./AnnouncementsAdministration.component.less']
})
export class AnnouncementsAdministrationComponent implements OnInit {
  @ViewChild('select') select: MatSelect;

  files: Announcement[] = [];
  currentFile: File;
  users: any[];
  filteredHouseDevelopments: any[];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];
  allBuildingsSelected: boolean;

  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  flatsFrom = new FormControl();
  usersFrom = new FormControl();

  usersToSendData: any[];

  constructor(public blobService: BlobService,
    private alertifyService: AlertifyService,
    private userService: UserService,
    private authService: AuthService,
    private datePipe: DatePipe,
    private announcementService: AnnouncementService
  ) {
  }

  displayedColumns: string[] = ['Name', 'Email', 'Address'];

  toggleAllSelection(){
    if (this.allBuildingsSelected) {
      this.select.options.forEach((item: MatOption) => item.select());
    } else {
      this.select.options.forEach((item: MatOption) => item.deselect());
    }
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

  filterFlats($event) {
    console.log($event);
console.log(this.buildingsFrom);
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

  showUsersList() {
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
    console.log($event);
    this.prepareFilesList($event);
  }

  fileBrowseHandler(files): void {
    console.log(files);
    this.prepareFilesList(files);
  }

  prepareFilesList(files: Array<File>): void {
    for (const item of files) {
      console.log((item as File).type);
      if (item.type === 'application/pdf') {
let announcement = new Announcement();
announcement.file = item;
        this.files.push(announcement);
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
  deleteFile(index: number): void {
    this.files.splice(index, 1);
  }
  insertAnnouncements() {
    console.log(this.usersToSendData);
    console.log(this.files);

    this.files.forEach(async element => {
      const currentFileDate = new Date();
      const fileDate = this.datePipe.transform(currentFileDate, 'yyyyMMddHHmmss');
      if (element) {

        const newName = this.authService.decodedToken.nameid + '_' + fileDate + '_' + element.file.name;
        this.currentFile = new File([element.file], newName);

        const response = await this.blobService.uploadFile(this.currentFile, FileTypeEnum.ANNOUNCEMENT);

        if (response._response.status === 201) {
          console.log(response);
          const fileName = this.currentFile.name;
          const id = this.authService.decodedToken.nameid;
          const req: any = this.blobService.createRequestForAddingAnnouncement(fileName, id, this.usersToSendData.map(p => p.userId),element.description);
          console.log(req);
          this.announcementService.insertAnnouncement(req).subscribe(data => {
            console.log(data);
            this.usersToSendData = [];
            this.filteredHouseDevelopments = [];
            this.filteredUsers = [];
            this.filteredBuildings = [];
            this.filteredFlats = [];
            this.files = [];

            this.alertifyService.success('Pliki zostały wstawione!');
          }
          );
        }
      }
    });
  }
}
