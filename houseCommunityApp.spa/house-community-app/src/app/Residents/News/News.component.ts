import { HttpClient } from '@angular/common/http';
import { AfterViewInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from '../../_services/alertify.service';
import { AnnouncementService } from '../../_services/announcement.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-News',
  templateUrl: './News.component.html',
  styleUrls: ['./News.component.scss']
})
export class NewsComponent implements OnInit, AfterViewInit  {
  
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('pdfViewer') pdfViewer;
  announcements: any = [];
  dataSource = new MatTableDataSource(this.announcements);
  displayedColumns: string[] = ['Name', 'Description', 'creationDate', 'Author', 'Preview'];
  constructor(private authService: AuthService, private alertifyService: AlertifyService, private announcementService: AnnouncementService, private http: HttpClient) { }

  ngOnInit() {

    this.announcementService.getAnnouncementsForUser(this.authService.decodedToken.nameid).subscribe(data => {
      let announcements = (data as any).announcements as any[];
      console.log(announcements);
      console.log(this.dataSource);
      this.dataSource.data = announcements;

    });
  }
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  
  }
  preview(imageUrl: string) {
    console.log(imageUrl);
  }


  public openPdf(fileUrl: string) {
    // url can be local url or remote http request to an api/pdf file. 
    // E.g: let url = "assets/pdf-sample.pdf";
    // E.g: https://github.com/intbot/ng2-pdfjs-viewer/tree/master/sampledoc/pdf-sample.pdf
    // E.g: http://localhost:3000/api/GetMyPdf
    // Please note, for remote urls to work, CORS should be enabled at the server. Read: https://enable-cors.org/server.html

    this.announcementService.displayBlobImage(fileUrl).subscribe(

      (blob) => {
        console.log(blob);
        this.pdfViewer.pdfSrc = (blob); // pdfSrc can be Blob or Uint8Array
        this.pdfViewer.refresh(); // Ask pdf viewer to load/reresh pdf
        console.log(this.pdfViewer);
      }
    );




  }
}
