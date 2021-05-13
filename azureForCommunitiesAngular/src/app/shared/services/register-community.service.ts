import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CommunityModel } from '../models/community-model';
import { TokenHolder } from '../models/tokenHolder.model';

@Injectable({
  providedIn: 'root'
})
export class RegisterCommunityService {

  constructor(private http : HttpClient) { }

  sendCommunity(community:CommunityModel)
  {
    let tkh : TokenHolder = localStorage.tkh == null || JSON.parse(localStorage.tkh) || new TokenHolder();
    let url = environment.apiUrl+"/api/CommunitiesAPI?userId="+tkh.id;    
    return this.http.post(url, community);
  }
}
