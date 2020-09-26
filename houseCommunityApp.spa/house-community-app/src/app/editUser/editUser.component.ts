import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-editUser',
  templateUrl: './editUser.component.html',
  styleUrls: ['./editUser.component.scss']
})
export class EditUserComponent implements OnInit {

  phoneDisabled: boolean;
  emailDisabled: boolean;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  birthDate: Date;
  constructor(public authService: AuthService, public userService: UserService) {

  }

  ngOnInit() {
    let user: any;
    this.userService.getUser(this.authService.decodedToken.nameid).subscribe(data => {
      user = data;
      this.phoneDisabled = true;
      this.emailDisabled = true;
      this.firstName = user.firstName;
      this.lastName = user.lastName;
      this.birthDate = user.birthdate;
      this.phoneNumber = user.phoneNumber;
      this.email = user.email;
    });
  }

  public editEmail() {
    this.emailDisabled = !this.emailDisabled;
  }
  public editPhone() {
    this.phoneDisabled = !this.phoneDisabled;
  }
}