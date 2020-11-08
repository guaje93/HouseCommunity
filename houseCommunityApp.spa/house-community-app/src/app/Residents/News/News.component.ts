import { HttpClient } from '@angular/common/http';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';
import { AnnouncementService } from '../../_services/announcement.service';
import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-News',
  templateUrl: './News.component.html',
  styleUrls: ['./News.component.scss']
})
export class NewsComponent implements OnInit {
  
  @ViewChild('pdfViewer') pdfViewer;
  displayedColumns: string[] = ['Name', 'Description', 'CreationDate', 'Author', 'Preview'];
  announcements: any;
  constructor(private authService: AuthService, private alertifyService: AlertifyService, private announcementService: AnnouncementService, private http: HttpClient) { }

  ngOnInit() {

    this.announcementService.getAnnouncementsForUser(this.authService.decodedToken.nameid).subscribe(data => {
      let announcements = (data as any).announcements as any[];
      console.log(announcements);
      this.announcements = announcements;

    });
  }

  preview(imageUrl: string) {
    console.log(imageUrl);
  }


  public openPdf() {
    let url = "https://housecommunitystorage.blob.core.windows.net/announcementcontainer/TestAnnouncement.pdf";
    // url can be local url or remote http request to an api/pdf file. 
    // E.g: let url = "assets/pdf-sample.pdf";
    // E.g: https://github.com/intbot/ng2-pdfjs-viewer/tree/master/sampledoc/pdf-sample.pdf
    // E.g: http://localhost:3000/api/GetMyPdf
    // Please note, for remote urls to work, CORS should be enabled at the server. Read: https://enable-cors.org/server.html

    this.announcementService.displayBlobImage(url).subscribe(

      (blob) => {
        console.log(blob);
        this.pdfViewer.pdfSrc = (blob); // pdfSrc can be Blob or Uint8Array
        this.pdfViewer.refresh(); // Ask pdf viewer to load/reresh pdf
        console.log(this.pdfViewer);
      }
    );




  }
}
