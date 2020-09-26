import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import {MatDialog} from '@angular/material/dialog';
import { EditUserComponent } from '../editUser/editUser.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};

  constructor(
    public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router,
    public dialog: MatDialog
  ) {}
  ngOnInit() {}


  loggedOut() {
    localStorage.removeItem("token");
    this.alertifyService.message("Logged out!");
    this.router.navigate(["/login"]);
  }

  openDialog() {
    const dialogRef = this.dialog.open(EditUserComponent, {height: '85%', width: '70%'});

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

}
