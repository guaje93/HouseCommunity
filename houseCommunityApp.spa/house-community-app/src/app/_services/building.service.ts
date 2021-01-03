import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {
  baseUrl = "https://housecommunityapp.azurewebsites.net/api/building/";

  constructor(private http: HttpClient) {}

getAllflats(id: number){
  return this.http.get(this.baseUrl + 'get-flats/' + id).pipe(retry(3));
}

getFlatsForFilter(id: number){
  return this.http.get(this.baseUrl + 'get-flats-for-filtering/' + id).pipe(retry(3));
}

getBuildings(){
  return this.http.get(this.baseUrl + 'get-buildings/').pipe(retry(3));
}

getFlatResidents(id: number){
  return this.http.get(this.baseUrl + 'get-flat-residents/' + id).pipe(retry(3));
}

}
