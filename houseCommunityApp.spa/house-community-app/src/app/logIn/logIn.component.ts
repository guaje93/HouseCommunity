import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { Router } from "@angular/router";
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-logIn',
  templateUrl: './logIn.component.html',
  styleUrls: ['./logIn.component.css']
})
export class LogInComponent implements OnInit {
  username: FormControl;
  password: FormControl;
  loginFormGroup: FormGroup;

  constructor(
    public authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router

  ) { }
  hide: boolean = true;
  ngOnInit() {
    this.loginFormGroup = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  logIn() {
    if (this.loginFormGroup.valid) {

      let model: any = {};
      model.username = this.loginFormGroup.controls.username.value;
      model.password = this.loginFormGroup.controls.password.value;
      this.authService.login(model).subscribe(

        next => {
          console.log(next);
          this.alertifyService.success("Zalogowano " + model.username);
          this.router.navigate(['home']);
        },
        error => {
          if (error.error)
            this.alertifyService.error(error.error);
        },
        () => {

        });
    }

    else {
      this.alertifyService.error("Nie podano wszystkich danych");
    }
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

}
