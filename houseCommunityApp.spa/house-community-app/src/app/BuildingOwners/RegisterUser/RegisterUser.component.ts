import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BuildingService } from 'src/app/_services/building.service';
import { MediaService } from 'src/app/_services/media.service';
import { UserService } from 'src/app/_services/user.service';
import { RegisterFormComponent } from '../registerForm/registerForm.component';

@Component({
  selector: 'app-RegisterUser',
  templateUrl: './RegisterUser.component.html',
  styleUrls: ['./RegisterUser.component.scss']
})
export class RegisterUserComponent implements OnInit {

  users: any[];

  filteredHouseDevelopments: any[] = [];
  filteredBuildings: any[];
  filteredFlats: any[];
  filteredUsers: any[];
  houseDevelopmentsFrom = new FormControl();
  buildingsFrom = new FormControl();
  usersToSendData: any[];
  
  displayedColumns: string[] = ['Building', 'Add','Generate'];
  displayedUsersColumns: string[] = ['UserName'];

  constructor(private userService: UserService, 
    private buildingService: BuildingService,
    private alertifyService: AlertifyService,
    private authService: AuthService, 
    private mediaService: MediaService, 
    public dialog: MatDialog) { }

  ngOnInit() {
this.getAllusers();
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

  
  getAllusers() {
    this.users = [];
    this.filteredHouseDevelopments = [];

    let id = this.authService.decodedToken.nameid;
    this.buildingService.getAllflats(id).subscribe(data => {
      this.users = data as any[];
      console.log(this.users);
this.showFlatsList();
    });
  }

  showFlatsList() {
    this.usersToSendData = this.users
      .map(flat => {
        let list: any = {};

        let flat1 =
        {
          id: flat.id,
          address: flat.address,
          usersList: flat.residents
        };
        console.log(flat);
        return flat1;
      });
  }

  openDialog(id: number): void {
    const dialogRef = this.dialog.open(RegisterFormComponent, {
      width: '60%',
      data: id
    });

    dialogRef.afterClosed().subscribe(() => {
      this.getAllusers();
      console.log('The dialog was closed');
    });
  }

}
