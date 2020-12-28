import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { Role } from '../../Model/Role';
import { ImagePreviewComponent } from 'src/app/residents/ImagePreview/ImagePreview.component';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { MediaService } from 'src/app/_services/media.service';
import { UserService } from 'src/app/_services/user.service';
import { BuildingService } from 'src/app/_services/building.service';

@Component({
  selector: 'app-MediaAdministration',
  templateUrl: './MediaAdministration.component.html',
  styleUrls: ['./MediaAdministration.component.less']
})
export class MediaAdministrationComponent implements OnInit {

  Role = Role;
  users: any[];
  totalWaitingForUserMedia: any[] = [];
  totalWaitingForBookMedia: any[] = [];
  filteredHouseDevelopments: any[] = [];
  filteredPeriods: any[];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];

  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  periodForm = new FormControl();

  usersToSendData: any[];


  constructor(private userService: UserService,
    private alertifyService: AlertifyService,
    private authService: AuthService,
    private mediaService: MediaService,
    private buildingService: BuildingService,
    public dialog: MatDialog
  ) {
  }

  ngOnInit() {
    this.getAllusers();
  }

  getCurrentPeriod() {
    let year = new Date().getFullYear();
    let month = new Date().getMonth();
    let period = '';
    if (month > 6)
      period = "H02" + year;
    else
      period = "H01" + year;
    console.log(period);
    return period;
  }

  generateEmptyMedia(flat: any) {
    console.log(flat);
    let model: any = {};
    model.flatId = flat.flatId;
    model.administratorId = this.authService.decodedToken.nameid;

    this.mediaService.createEmptyMedia(model).subscribe(
      data => {
        this.getAllusers();
        this.alertifyService.success('Dodano')
      },
      error => this.alertifyService.error('Błąd'));
  }

  getAllusers() {
    this.users = [];
    this.filteredHouseDevelopments = [];
    let period = this.getCurrentPeriod();
    this.userService.getAllusers().subscribe(data => {
      this.users = data as any[];
      console.log(data);

      this.users.forEach(user => {
        this.mediaService.getMediaForFlat(user.flatId).subscribe(mediaData => {
          console.log(mediaData);
          user.mediaList = mediaData;
          user.mediaList.forEach(media => {
            if (media.mediaType == 1)
              media.mediaType = "Woda zimna";
            else if (media.mediaType == 2)
              media.mediaType = "Woda ciepła";
            else if (media.mediaType == 3)
              media.mediaType = "Ogrzewanie";
          });
          console.log(mediaData);
        }

        );

        this.buildingService.getFlatResidents(user.flatId).subscribe(flatData => {
          console.log(flatData);
          user.residentsList = flatData;
        }

        );
      })
      console.log(this.users);
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

      this.totalWaitingForBookMedia = [];
      this.totalWaitingForUserMedia = [];
      this.users.forEach(p => {
        this.mediaService.getMediaForFlat(p.flatId).subscribe(data => {
          console.log(data);
          if ((data as any[]).length > 0) {
            (data as any[]).forEach(element => {
              if (element.status === 0) this.totalWaitingForUserMedia.push(data);
              if (element.status === 1) this.totalWaitingForBookMedia.push(data);
            });
          }

          this.showFlatsList();
        })
      });
      console.log(this.users);
      console.log(this.filteredHouseDevelopments);
    });
  }

  displayedColumns: string[] = ['Building', 'Generate'];
  displayedMediaColumns: string[] = ['Type', 'Period', 'Value', 'LastValue', 'Accept'];

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
        user.mediaList.forEach(media => {
          if (!periods.includes(media.period))
            periods.push(media.period);

        });
      })
    
      if(!periods.includes(this.getCurrentPeriod())){
        periods.push(this.getCurrentPeriod());
      }
      this.filteredPeriods = periods.sort();
      this.periodForm.reset();
      this.usersToSendData = [];

  }


  openDialog(imageUrl: string): void {
    const dialogRef = this.dialog.open(ImagePreviewComponent, {
      width: '80%',
      data: imageUrl
    });

    dialogRef.afterClosed().subscribe(() => {
      console.log('The dialog was closed');
    });
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
          mediaList: flat.mediaList.filter(media => this.periodForm.value.includes(media.period)),
          residentsList: flat.residentsList
        };
        console.log(flat);
        return flat1;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.building === value.building && t.local === value.local
      ))).sort(
        function(a, b) {          
          if (a.building === b.building) {
             return a.local - b.local;
          }
          return a.building > b.building ? 1 : -1;
       }
      );
  }
  bookMedia(media: any) {
    let model: any = {

      userId: this.authService.decodedToken.nameid,
      mediaId: media.id,
      currentValue: media.currentValue
    }
    this.mediaService.bookMedia(model).subscribe(
      data => {
        this.getAllusers();
        this.alertifyService.success('Zaksięgowano!');

      },
      error => {
        this.alertifyService.error('Błąd! Nie zaksięgowano!');
      }
    );
  }

  public decodePeriod(encodedPeriod: string) {

    let year = encodedPeriod.substring(3);
    if (encodedPeriod.startsWith('H01')) {
      return "Rok " + year + ", półrocze 1";
    }
    else {
      return "Rok " + year + ", półrocze 2";
    }
  }

  public getResidentsToContact(residents: any[]){
let result = '';
    residents.forEach(element => {
      result = result + element.firstName + ' ' + element.lastName + '\n';
      result = result + 'Mail: ' + element.email + '\n';
      if(element.phoneNumber)
      result = result + 'Tel:' + element.phoneNumber + '\n';
      result = result + '\n';
    });
    return result;
  }

  unlockMedia(media: any) {
    let model: any = {

      userId: this.authService.decodedToken.nameid,
      mediaId: media.id,
    }
    this.mediaService.unlockMedia(model).subscribe(
      data => {
        this.getAllusers();
        this.alertifyService.success('Cofnięto!');

      },
      error => {
        this.alertifyService.error('Błąd! Nie cofnięto!');
      }
    );
  }
}


