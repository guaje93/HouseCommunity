import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-registerForm',
  templateUrl: './registerForm.component.html',
  styleUrls: ['./registerForm.component.less']
})
export class RegisterFormComponent implements OnInit {

  public option: string = "0";
  users: any[];

  constructor(
    public dialogRef: MatDialogRef<RegisterFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: number, private alertifyService: AlertifyService, private authService: AuthService, private userService: UserService) { }
    
    
    onNoClick(): void {
      this.dialogRef.close();
    }
  ngOnInit() {
    console.log(this.data);
    this.userService.getResidents().subscribe(data =>{
this.users = data as any[];
    console.log(this.users);
  })
  }
  
  usersForm: FormControl = new FormControl('',[Validators.required]);
  firstName = new FormControl('', [Validators.required, Validators.minLength(2)]);
  lastName = new FormControl('', [Validators.required, Validators.minLength(3)]);
  email = new FormControl('', [Validators.required, Validators.email]);

  getErrorMessage() {
    if (this.email.hasError('required')) {
      return 'You must enter a value';
    }

    return this.email.hasError('email') ? 'Not a valid email' : '';
  }

  addNewUser() {
    if (!this.email.invalid && !this.firstName.invalid && !this.lastName.invalid) {

      let model: any = {};
      model.firstName = this.firstName.value;
      model.lastName = this.lastName.value;
      model.email = this.email.value;
      model.flatId = this.data;
      model.userId = this.authService.decodedToken.nameid;
      this.authService.registerNewUser(model).subscribe(data => {
        console.log(data);
        this.alertifyService.success("Dodano użytkownika");
        this.dialogRef.close();
      },
        error => {
          if (error.error) {
            this.alertifyService.error(error.error);
          }
        })
      console.log(model);
    }
    else {
      this.alertifyService.error('Wprowadź poprawne dane');
    }
  }

  addExistingUser() {
    if (!this.usersForm.invalid) {

      let model: any = {};
      model.email = this.usersForm.value;
      model.flatId = this.data;
      model.userId = this.authService.decodedToken.nameid;
      this.authService.registerExistingUser(model).subscribe(data => {
        console.log(data);
        this.alertifyService.success("Dodano użytkownika");
        this.dialogRef.close();
      },
        error => {
          if (error.error) {
            this.alertifyService.error(error.error);
          }
        })
      console.log(model);
    }
    else {
      this.alertifyService.error('Wprowadź poprawne dane');
    }
  }

  optionChange() {

  }
}