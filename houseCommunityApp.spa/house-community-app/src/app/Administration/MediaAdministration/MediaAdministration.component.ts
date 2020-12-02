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

@Component({
  selector: 'app-MediaAdministration',
  templateUrl: './MediaAdministration.component.html',
  styleUrls: ['./MediaAdministration.component.less']
})
export class MediaAdministrationComponent implements OnInit {

  Role = Role;
  users: any[];
  totalNotGeneratedMedia: any[] = [];
  totalBookedMedia: any[];
  totalWaitingForUserMedia: any[] = [];
  totalWaitingForBookMedia: any[] = [];
  filteredHouseDevelopments: any[] = [];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];

  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();

  usersToSendData: any[];


  constructor(private userService: UserService, 
              private alertifyService: AlertifyService,
              private authService: AuthService, 
              private mediaService: MediaService, 
              public dialog: MatDialog
              ) {
  }

  ngOnInit() {
    this.getAllusers();
  }

  generateEmptyMedia(flat: any) {
    console.log(flat);
    let model: any = {};
    model.flatId = flat.flatId;

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

    this.userService.getAllusers().subscribe(data => {
      this.users = data as any[];

      this.users.forEach(user => {
        this.mediaService.getMediaForFlat(user.flatId).subscribe(mediaData => {
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

      this.totalBookedMedia = [];
      this.totalNotGeneratedMedia = [];
      this.totalWaitingForBookMedia = [];
      this.totalWaitingForUserMedia = [];
      this.users.forEach(p => {
        this.mediaService.getMediaForFlat(p.flatId).subscribe(data => {
          console.log(data);
          if ((data as any[]).length === 0) this.totalNotGeneratedMedia.push(data);
          if ((data as any[]).length > 0) {
            (data as any[]).forEach(element => {
              if (element.status === 0) this.totalWaitingForUserMedia.push(data);
              if (element.status === 1) this.totalWaitingForBookMedia.push(data);
              if (element.status === 2) this.totalBookedMedia.push(data);
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
  displayedMediaColumns: string[] = ['Type', 'StartDate', 'EndDate', 'Value', 'Accept'];

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
          mediaList: flat.mediaList
        };
        console.log(flat);
        return flat1;
      }).filter((value, index, self) => index === self.findIndex((t) => (
        t.flatId === value.flatId && t.building === value.building && t.local === value.local
      )));
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


