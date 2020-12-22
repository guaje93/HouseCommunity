import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-changePassword',
  templateUrl: './changePassword.component.html',
  styleUrls: ['./changePassword.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  currentPassword  = new FormControl('', [Validators.required]);
  newPassword  = new FormControl('', [Validators.required, Validators.minLength(6)]);
  confirmPassword  = new FormControl('', [Validators.required, Validators.minLength(6)]);

  hideCurrentPassword: boolean = true;
  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;
  changePasswordForm: FormGroup;
  successMessage: string;
  constructor(private authService: AuthService, private fb: FormBuilder, private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {

  }

  changePassword() {
    if (this.Validate(this.changePasswordForm)) {

      this.authService.changePassword(this.changePasswordForm.value).subscribe(
        data => {
          this.changePasswordForm.reset();
          this.successMessage = "Zmieniono hasło!";
          this.alertifyService.success(this.successMessage);
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

  onPasswordInput() {
    if (this.confirmPassword.value !== this.newPassword.value)
      this.confirmPassword.setErrors([{'passwordMismatch': true}]);
    else
      this.confirmPassword.setErrors(null);
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
