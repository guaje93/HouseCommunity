import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-registerForm',
  templateUrl: './registerForm.component.html',
  styleUrls: ['./registerForm.component.less']
})
export class RegisterFormComponent implements OnInit {



    constructor(
      public dialogRef: MatDialogRef<RegisterFormComponent>,
      @Inject(MAT_DIALOG_DATA) public data: number, private alertifyService: AlertifyService, private authService: AuthService) {}
  
    onNoClick(): void {
      this.dialogRef.close();
    }
    ngOnInit() {
      console.log(this.data);
    }

    firstName = new FormControl('', [Validators.required, Validators.minLength(2)]);
    lastName  = new FormControl('', [Validators.required, Validators.minLength(3)]);
    userName  = new FormControl('', [Validators.required, Validators.minLength(3)]);
    password  = new FormControl('', [Validators.required, Validators.minLength(6)]);
    confirmPassword  = new FormControl('', [Validators.required]);
    email = new FormControl('', [Validators.required, Validators.email]);
    hide = true;
    hideConfirmedPassword = true;
    getErrorMessage() {
      if (this.email.hasError('required')) {
        return 'You must enter a value';
      }
  
      return this.email.hasError('email') ? 'Not a valid email' : '';
}

addUser(){
  if(!this.password.invalid && !this.confirmPassword.invalid && !this.email.invalid && !this.firstName.invalid && !this.lastName.invalid)
  {

    let model: any = {};
    model.firstName = this.firstName.value;
    model.lastName = this.lastName.value;
    model.userName = this.userName.value;
    model.email = this.email.value;
    model.password= this.password.value;
    model.confirmPassword = this.confirmPassword.value;
    model.flatId = this.data;
    this.authService.registerUser(model).subscribe(data =>
      {
        console.log(data);
        this.alertifyService.success("Dodano użytkownika");
        this.dialogRef.close();
      })
    console.log(model); 
  } 
  else{
    this.alertifyService.error('Wprowadź poprawne dane');
  }
}
onPasswordInput() {
  if (this.confirmPassword.value !== this.password.value)
    this.confirmPassword.setErrors([{'passwordMismatch': true}]);
  else
    this.confirmPassword.setErrors(null);
}

}