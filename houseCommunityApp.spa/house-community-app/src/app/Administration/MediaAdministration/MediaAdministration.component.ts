import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-MediaAdministration',
  templateUrl: './MediaAdministration.component.html',
  styleUrls: ['./MediaAdministration.component.less']
})
export class MediaAdministrationComponent implements OnInit {

  constructor(private userService: UserService) { 
  }
  
  ngOnInit() {
    this.getAllusers();
  }

  getAllusers() {
    this.userService.getAllusers().subscribe(data => {
      console.log(data);
    });
  }
  
}
  