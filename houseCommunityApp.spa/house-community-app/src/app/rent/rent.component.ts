import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-rent',
  templateUrl: './rent.component.html',
  styleUrls: ['./rent.component.scss']
})
export class RentComponent implements OnInit {

  displayedColumns: string[] = ['Name', 'Value', 'Date','Details', 'Payment'];
  
  constructor() { }

  ngOnInit() {
  }

}
