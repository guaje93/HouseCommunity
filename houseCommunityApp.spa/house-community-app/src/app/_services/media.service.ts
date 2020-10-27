import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MediaService {

  baseUrl = 'http://localhost:5000/api/media/';
  constructor(private http: HttpClient) {}

  getMediaForUser(id: number) {
    return this.http.get(this.baseUrl + id);
  }

  addMediaForUser(model: any){
    return this.http.post(this.baseUrl + 'add-media', model);
  }

displayBlobImage(url: string){
  return this.http.get(url, { responseType: 'blob' });
}


}
