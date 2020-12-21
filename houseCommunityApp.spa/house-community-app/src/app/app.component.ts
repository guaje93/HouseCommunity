import { Component } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from './Model/Role';
import { User } from './Model/user';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  constructor(private authService: AuthService) { }
  jwtHelper = new JwtHelperService();
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const role = +localStorage.getItem('role');
    console.log(role);
    if (token) {
      this.authService.user = new User();
      this.authService.user.token = token;
      this.authService.user.role = role;
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);   
    }
  }
    title = 'house-community-app';
  }
