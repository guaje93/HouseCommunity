import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AnnouncementService {
  baseUrl = "https://housecommunityapp.azurewebsites.net/api/announcement/";

  constructor(private http: HttpClient) {}

  getAnnouncementsForUser(userId: number) {
    return this.http.get(this.baseUrl + 'get-announcements-for-user/' + userId);
  }

  displayBlobImage(url: string){
    return this.http.get(url, { responseType: 'blob' });
  }

  insertAnnouncement(model: any){
    return this.http.post(this.baseUrl + 'insert-announcements', model);
  }
}
