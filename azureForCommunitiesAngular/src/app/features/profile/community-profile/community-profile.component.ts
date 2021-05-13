import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Admission } from 'src/app/shared/models/admission.model';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { AdmissionValidation } from 'src/app/shared/models/api-requests/admission-validation.model';
import { MultiData } from 'src/app/shared/models/api-responses/multi-data.model';
import { CommunityModel } from 'src/app/shared/models/community-model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { CommunityService } from 'src/app/shared/services/community.service';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { NewCommunityDialogComponent } from '../dialogs/new-community-dialog/new-community-dialog.component';

@Component({
  selector: 'app-community-profile',
  templateUrl: './community-profile.component.html',
  styleUrls: ['./community-profile.component.css']
})
export class CommunityProfileComponent implements OnInit {

  @Input() admissions?:Array<Admission>;
  @Input() pendingAdmissions?:Array<Admission>;

  public admissionsCommunities?:Array<CommunityModel>;
  public pendingAdmissionsCommunities?:Array<CommunityModel>;

  constructor(public dialog: MatDialog, public CommunityService:CommunityService, private router:Router) 
  {
    this.admissionsCommunities = new Array();
    this.pendingAdmissionsCommunities = new Array();
  }

  ngOnInit(): void {
  }

  ngOnChanges(changements: SimpleChanges) {
    console.log(changements); //Valeur actuelle du libellé (après le changement)
    if(this.admissions != null)
    {
      for (const admission of this.admissions) {
        this.getCommunity(admission.community!, this.admissionsCommunities!);
      }
    }

    if(this.pendingAdmissions != null)
    {
      for (const admission of this.pendingAdmissions) {
        this.getCommunity(admission.community!, this.pendingAdmissionsCommunities!);
      }
    }

  }

  getAdmissions():Array<Admission>
  {
    return this.admissions || new Array<Admission>();
  }

  getPendingAdmissions():Array<Admission>
  {
    return this.pendingAdmissions || new Array<Admission>();
  }

  onClick()
  {
    //this.dialog.open(NewCommunityDialogComponent);
    this.router.navigate(["/createCommunity"]);
  }

  acceptInvite(community:number)
  {
    let command = new AdmissionValidation();
    command.communityId = community;
    let tkh : TokenHolder = localStorage.tkh == null || JSON.parse(localStorage.tkh) || new TokenHolder();
    command.userId = tkh.id;
    this.CommunityService.acceptInvitation(command).subscribe(
      (res:any) => {
        let index = this.pendingAdmissionsCommunities!.findIndex(x => x.id === community);
        if (index > -1) {
          let coms = this.pendingAdmissionsCommunities?.splice(index, 1);
          this.admissionsCommunities!.push(coms![0]);
       }
        
      },
      err => {
        console.log(err);
      }
    );
  }

  getCommunity(id:number, admissionArray:Array<CommunityModel>)
  {
    this.CommunityService.getCommunityDetails(id, false).subscribe(
      (res:any) => {
        let community = res as CommunityModel;
        admissionArray?.push(community);
      },
      err => {
        console.log(err);
      }
    )
  }

}
