import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DamageService {

  baseUrl = 'https://housecommunityapp.azurewebsites.net/api/damage/';
  constructor(private http: HttpClient) {}

  addDamage(model: any){
    return this.http.post(this.baseUrl,model);
  }
  updateImageInfo(model: any){
    return this.http.post(this.baseUrl + 'update-image',model);
  }

  getDamagesForHouseManager(id: number){
    return this.http.get(this.baseUrl + 'get-damages-for-building/' +id);

  }

  getFixedDamagesForHouseManager(id: number){
    return this.http.get(this.baseUrl + 'get-fixed-damages-for-building/' +id);

  }
  
  markDamageAsFixed(id: number){
    return this.http.post(this.baseUrl + 'fix-damage/' ,id);

  }

  markDamageAsNotFixed(id: number){
    return this.http.post(this.baseUrl + 'revert-fix/' ,id);

  }
}
