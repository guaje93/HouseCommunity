import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-resetPassword',
  templateUrl: './resetPassword.component.html',
  styleUrls: ['./resetPassword.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetToken: null;
  CurrentState: any;
  isPasswordReset= false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute,
    private fb: FormBuilder ) {
    this.CurrentState = 'Wait';
    this.route.params.subscribe(params => {
      this.resetToken = params.token;
      console.log(this.resetToken);
      this.VerifyToken();
    });
  }

  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;
  changePasswordForm: FormGroup;

  ngOnInit() {
    this.changePasswordForm = this.fb.group({
      resettoken: [this.resetToken],
      newPassword  : new FormControl('', [Validators.required, Validators.minLength(6)]),
      confirmPassword  : new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  VerifyToken() {
    this.authService.ValidPasswordToken({ resettoken: this.resetToken }).subscribe(
      data => {
        this.CurrentState = 'Verified';
      },
      err => {
        this.CurrentState = 'NotVerified';
        this.alertifyService.error('Token niepoprawny!');
      }
    );
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
  
  ResetPassword() {
    if (!this.isPasswordReset && this.Validate(this.changePasswordForm)) {
      this.isPasswordReset = true;
      this.authService.newPassword(this.changePasswordForm.value).subscribe(
        data => {
          this.changePasswordForm.reset();
          this.alertifyService.success("Ustawiono nowe hasło!");
          this.router.navigate(['login']);
        },
        err => {
          if (err.error) {
            this.alertifyService.error(err.error);
      this.isPasswordReset = false;

          }
        }
      );
    } 
    else { 
      this.alertifyService.error('Hasło niepoprawne');  
    }
  }
}