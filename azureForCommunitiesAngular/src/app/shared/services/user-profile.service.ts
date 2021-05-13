import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserProfile } from '../models/user-profile.model';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {
  public errorMessage?: string;
  public succesMessage?: string;

  public UserProfile: UserProfile;
  private http : HttpClient;

  constructor(http:HttpClient)
  {
    this.http = http;
    this.UserProfile = new UserProfile();
  }

  getProfile(id:string)
  {
    return this.http.get(`${environment.apiUrl}/api/UsersAPI/private/${id}`);
  }

  getCommunityProfile(id:Number, full:boolean = false)
  {
    return this.http.get(`${environment.apiUrl}/api/CommunitiesAPI/${id}?full=${String(full)}`);
  }

  getUserAnchors(id:string)
  {
    return this.http.get(`${environment.apiUrl}/api/AnchorsAPI/user?UserId=${id}`);
  }

  getAnchorsInCommunity(id:Number)
  {
    return this.http.get(`${environment.apiUrl}/api/AnchorsAPI/inCommunity?CommunityId=${id}`);
  }

  getAnchorsByCommunity(id:Number)
  {
    return this.http.get(`${environment.apiUrl}/api/AnchorsAPI/byCommunity?CommunityId=${id}`);
  }
}
