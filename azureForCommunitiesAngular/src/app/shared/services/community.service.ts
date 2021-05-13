import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AdmissionRequest } from '../models/api-requests/admission-request.model';
import { AdmissionValidation } from '../models/api-requests/admission-validation.model';

@Injectable({
  providedIn: 'root'
})

export class CommunityService {

  constructor(public http : HttpClient) { }

  inviteUser(request:AdmissionRequest)
  {
    return this.http.post(environment.apiUrl+"/api/AdmissionAPI/community", request);
  }

  getCommunityDetails(id:number, full:boolean)
  {
    return this.http.get(environment.apiUrl+"/api/CommunitiesAPI/"+id+"?full="+full);
  }

  acceptInvitation(request:AdmissionValidation)
  {
    return this.http.post(environment.apiUrl+"/api/AdmissionAPI", request);
  }

  getAnchorsByCommunity(id:Number)
  {
    return this.http.get(environment.apiUrl+"/api/AnchorsAPI/byCommunity?CommunityId="+id);
  }

}
