import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Role } from 'src/app/Model/Role';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  Role: Role;
  constructor(
    public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
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
