import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BlobService } from 'src/app/_services/blob.service';
import { UserService } from 'src/app/_services/user.service';
import { FormControl } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { DatePipe } from '@angular/common';
import { AnnouncementService } from 'src/app/_services/announcement.service';
import { FileTypeEnum } from 'src/app/Model/fileTypeEnum';
import { Announcement } from 'src/app/Model/announcement';
import { MatSelect } from '@angular/material/select';
import { MatOption } from '@angular/material/core';
import { BuildingService } from 'src/app/_services/building.service';

@Component({
  selector: 'app-AnnouncementsAdministration',
  templateUrl: './AnnouncementsAdministration.component.html',
  styleUrls: ['./AnnouncementsAdministration.component.less']
})
export class AnnouncementsAdministrationComponent implements OnInit {
  @ViewChild('selectBuildings') selectBuildings: MatSelect;
  @ViewChild('selectFlats') selectFlats: MatSelect;
  @ViewChild('selectUsers') selectUsers: MatSelect;
  @ViewChild('fileDropRef', { static: false }) fileDropEl: ElementRef;

  files: Announcement[] = [];
  currentFile: File;
  users: any[];
  filteredHouseDevelopments: any[];
  filteredBuildings: any[];
  filteredFlats: any[];
  allBuildingsSelected: boolean;
  allFlatsSelected: boolean;
  allUsersSelected: boolean;

  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  flatsFrom = new FormControl();
  usersFrom = new FormControl();

  usersToSendData: any[];

  constructor(public blobService: BlobService,
    private alertifyService: AlertifyService,
    private buildingService: BuildingService,
    private authService: AuthService,
    private datePipe: DatePipe,
    private announcementService: AnnouncementService
  ) {
  }

  displayedColumns: string[] = ['Name', 'Email', 'Address'];

  toggleAllBuildingsSelection() {
    if (this.allBuildingsSelected) {
      this.selectBuildings.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectBuildings.options.forEach((item: MatOption) => item.deselect());
      this.usersToSendData = [];
      this.filteredFlats = [];
    }
    this.flatsFrom.reset();

    this.showUsersList();
  }

  toggleAllFlatsSelection() {
    if (this.allFlatsSelected) {
      this.selectFlats.options.forEach((item: MatOption) => item.select());
    } else {
      this.selectFlats.options.forEach((item: MatOption) => item.deselect());
      this.usersToSendData = [];

    }
  }



  filterBuildings($event) {
    this.filteredFlats = [];

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

      this.buildingsFrom.reset();
      this.flatsFrom.reset();

      this.allBuildingsSelected = false;
      this.allFlatsSelected = false;
  }

  filterFlats($event) {
    console.log($event);
    console.log(this.buildingsFrom);
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
      ))).sort((n1, n2) => {
        if (n1.localNumber > n2.localNumber) {
          return 1;
        }

        if (n1.localNumber < n2.localNumber) {
          return -1;
        }

        return 0;
      });;
    this.flatsFrom.reset();
    this.allFlatsSelected = false;
    this.showUsersList();
  }

  showUsersList() {
    console.log(this.flatsFrom);
    this.usersToSendData = this.users
      .filter(user => this.flatsFrom.value.includes(user.flatId))
      .map(user => {
        let flat =
        {
          userId: user.userId,
          name: user.name,
          userEmail: user.userEmail,
          address: user.address + ', m.' + user.localNumber
        };
        return flat;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.userId === value.userId && t.name === value.name && t.userEmail === value.userEmail && t.address === value.address
      )));
  }


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
        this.alertifyService.warning('Nieprawidłowy format pliku. \nZapisz plik w formacie png.');
      }
    }
    this.fileDropEl.nativeElement.value = '';
  }

  getAllusers() {
    this.users = [];
    this.filteredHouseDevelopments = [];

    this.buildingService.getFlatsForFilter(this.authService.decodedToken.nameid).subscribe(data => {
      this.users = data as any[];
      console.log(data);
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
          const fileName = element.file.name;
          const id = this.authService.decodedToken.nameid;
          const req: any = this.blobService.createRequestForAddingAnnouncement(fileName, newName, id, this.usersToSendData.map(p => p.userId), element.description);
          console.log(req);
          this.announcementService.insertAnnouncement(req).subscribe(data => {
            console.log(data);
            this.usersToSendData = [];
            this.filteredHouseDevelopments = [];
            this.filteredBuildings = [];
            this.filteredFlats = [];
            this.files = [];

            this.alertifyService.success('Plik został wstawiony!');
          }
          );
        }
      }
    });
  }
}
