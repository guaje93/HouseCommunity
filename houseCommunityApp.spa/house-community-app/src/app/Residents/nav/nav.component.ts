import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../../_services/alertify.service';
import { AuthService } from '../../_services/auth.service';
import {MatDialog} from '@angular/material/dialog';
import { EditUserComponent } from '../../editUser/editUser.component';
import { ChangePasswordComponent } from '../../changePassword/changePassword.component';
import { Role } from 'src/app/Model/Role';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
Role: Role;
  constructor(
    public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router,
    public dialog: MatDialog
  ) {}
  ngOnInit() {
  }


  loggedOut() {
    localStorage.removeItem("token");
    this.alertifyService.message("Logged out!");
    this.router.navigate(["/login"]);
  }

  openEditDataDialog() {
    const dialogRef = this.dialog.open(EditUserComponent, {height: '85%', width: '40%'});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  openChangePasswordDialog() {
    const dialogRef = this.dialog.open(ChangePasswordComponent, {height: '65%', width: '35%'});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  hasAdministrationAccess(){
    return this.authService.user.role === Role.Admin;
  }

  hasHouseManagerAccess(){
    return this.authService.user.role === Role.HouseManager;
  }

  hasUserAccess(){
    return this.authService.user.role === Role.User;
  }
}
