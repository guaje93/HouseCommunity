import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-forgetPassword',
  templateUrl: './forgetPassword.component.html',
  styleUrls: ['./forgetPassword.component.scss']
})
export class ForgetPasswordComponent implements OnInit {

  RequestResetForm: FormGroup;
  errorMessage: string;
  successMessage: string;
  IsMailSending = false;

  constructor(private authService: AuthService, private router: Router, private alertifyService: AlertifyService
  ) { }

  ngOnInit() {
    this.RequestResetForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
    });
  }

  RequestResetUser() {
    if (this.RequestResetForm.valid) {
      this.IsMailSending = true;
      this.authService.requestReset(this.RequestResetForm.value).subscribe(
        data => {
          this.RequestResetForm.reset();
          this.successMessage = "Wysłano wiadomość na podany adres!. Nastąpi przekierowanie do strony logowania.";
          this.alertifyService.success(this.successMessage);
          setTimeout(() => {
            this.successMessage = null;
            this.router.navigate(['login']);
          }, 2000);
        },
        err => {
          if (err.error) 
          {
      this.IsMailSending = false;
            console.log(err.error);
            this.alertifyService.error(err.error);
          }
        }
      );
    } else {

      this.alertifyService.error("Podaj poprawny email!");
    }
  }

}
