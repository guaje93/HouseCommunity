import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-changePassword',
  templateUrl: './changePassword.component.html',
  styleUrls: ['./changePassword.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
  changePasswordForm: FormGroup;
  successMessage: string;
  constructor(private authService: AuthService, private fb: FormBuilder, private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.changePasswordForm = this.fb.group(
      {
        id: [this.authService.decodedToken.nameid],
        currentPassword: ['', [Validators.required, Validators.minLength(4)]],
        newPassword: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(4)]],
      }
    );

  }

  changePassword() {
    if (this.Validate(this.changePasswordForm)) {

      this.authService.changePassword(this.changePasswordForm.value).subscribe(
        data => {
          this.changePasswordForm.reset();
          this.successMessage = "Zmieniono hasło!";
          this.alertifyService.success(this.successMessage);
          setTimeout(() => {
            this.successMessage = null;
          }, 2000);
        },
        err => {

          if (err.error.message) {
            console.log(err);
            this.alertifyService.error("Hasło nie zostało zmienione!");
          }
        }
      );
    }
  }

  Validate(passwordFormGroup: FormGroup):boolean {
    const new_password = passwordFormGroup.controls.newPassword.value;
    const confirm_password = passwordFormGroup.controls.confirmPassword.value;

    if (confirm_password.length <= 0) {
      this.alertifyService.error("Podaj nowe hasło");
      return false;
    }

    if (confirm_password !== new_password) {
      this.alertifyService.error("Podane hasło się różnią")
      return false;
    }

    return true;
  }

}
