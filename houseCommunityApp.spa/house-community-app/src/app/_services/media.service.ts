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

  updateMedia(model: any){
    return this.http.put(this.baseUrl + 'update-media', model);
  }

  bookMedia(model: any){
    return this.http.put(this.baseUrl + 'book-media', model);
  }

  unlockMedia(model: any){
    return this.http.put(this.baseUrl + 'unlock', model);
  }

displayBlobImage(url: string){
  return this.http.get(url, { responseType: 'blob' });
}

createEmptyMedia(model: any){
  return this.http.post(this.baseUrl + 'create-empty-media-entry', model);

}

getMediaForFlat(id: number){
  return this.http.get(this.baseUrl + 'media-for-flat/' + id);

}

}
