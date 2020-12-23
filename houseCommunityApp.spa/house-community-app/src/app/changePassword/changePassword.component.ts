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

  

  hideCurrentPassword: boolean = true;
  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;
  changePasswordForm: FormGroup;
  successMessage: string;
  constructor(private authService: AuthService, private fb: FormBuilder, private alertifyService: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.changePasswordForm = new FormGroup({
      currentPassword : new FormControl('', [Validators.required]),
      newPassword  : new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword  : new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  changePassword() {
    if (this.Validate(this.changePasswordForm)) {

      console.log(this.changePasswordForm.value);
      let model: any = {};
      model = this.changePasswordForm.value;
      model.id = this.authService.decodedToken.nameid;

      this.authService.changePassword(model).subscribe(
        data => {
          this.changePasswordForm.reset();
          this.successMessage = "Zmieniono hasło!";
          this.alertifyService.success(this.successMessage);
        },
        err => {
          if (err.error) {
            this.alertifyService.error(err.error);
          }
        }
      );
    }
  }

  onPasswordInput() {
    if (this.changePasswordForm.controls.confirmPassword.value !== this.changePasswordForm.controls.newPassword.value)
      this.changePasswordForm.controls.confirmPassword.setErrors([{'passwordMismatch': true}]);
    else
      this.changePasswordForm.controls.confirmPassword.setErrors(null);
  }

  Validate(passwordFormGroup: FormGroup):boolean {
    const new_password = passwordFormGroup.controls.newPassword.value;
    const confirm_password = passwordFormGroup.controls.confirmPassword.value;

    if (new_password.length <= 0) {
      this.alertifyService.error("Podaj nowe hasło");
      return false;
    }

    if (confirm_password !== new_password) {
      this.alertifyService.error("Podane hasła się różnią")
      return false;
    }

    if (confirm_password.length < 6) {
      this.alertifyService.error("Hasło jest za krótkie");
      return false;
    }

    return true;
  }

}
