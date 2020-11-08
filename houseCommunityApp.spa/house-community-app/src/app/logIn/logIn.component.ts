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
hide: boolean= true;
  ngOnInit() {
  }

  logIn() {
    this.authService.login(this.model).subscribe(
      next => {
        console.log(next);
        this.alertifyService.success("Zalogowano " +  this.model.username);
        if(next.userRole === 1){
          this.router.navigate(['home']);
        }
        else if(next.userRole === 2)
        {
          this.router.navigate(['homeAdministration']);
        }
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
