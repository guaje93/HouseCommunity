import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { Router } from "@angular/router";

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css']
})
export class LogInComponent implements OnInit {
  model: any = {};

  constructor(
    public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router

  ) { }

  ngOnInit() {
  }

  logIn() {
    this.authService.login(this.model).subscribe(
      next => {
        this.alertifyService.success("Zalogowano " +  this.model.username);
        this.router.navigate(['home']);
      },
      error => {
        this.alertifyService.error(error);
      },
      () => {

      }
    );
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

}
