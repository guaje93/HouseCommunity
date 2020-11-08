import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ChangePasswordComponent } from 'src/app/changePassword/changePassword.component';
import { EditUserComponent } from 'src/app/editUser/editUser.component';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-navAdministration',
  templateUrl: './navAdministration.component.html',
  styleUrls: ['./navAdministration.component.less']
})
export class NavAdministrationComponent implements OnInit {

  constructor(public authService: AuthService, private alertifyService: AlertifyService, private router: Router,
  public dialog: MatDialog) { }

  ngOnInit() {
  }

  loggedOut() {
    localStorage.removeItem("token");
    this.alertifyService.message("Logged out!");
    this.router.navigate(["/login"]);
  }

  openEditDataDialog() {
    const dialogRef = this.dialog.open(EditUserComponent, { height: '85%', width: '70%' });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

  openChangePasswordDialog() {
    const dialogRef = this.dialog.open(ChangePasswordComponent, { height: '65%', width: '35%' });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }

}
