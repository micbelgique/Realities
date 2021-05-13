import { Component, OnInit } from '@angular/core';
import {Location} from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CommunityModel } from 'src/app/shared/models/community-model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { CommunityService } from 'src/app/shared/services/community.service';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { InviteUserDialogComponent } from '../dialogs/invite-user-dialog/invite-user-dialog.component';
import { MultiData } from 'src/app/shared/models/api-responses/multi-data.model';
import { Anchor } from 'src/app/shared/models/anchor-model';

@Component({
  selector: 'app-community-profile-details',
  templateUrl: './community-profile-details.component.html',
  styleUrls: ['./community-profile-details.component.css']
})
export class CommunityProfileDetailsComponent implements OnInit {

  private communityId?:number;
  public CommunityModel?:CommunityModel;
  
  constructor(private route: ActivatedRoute, 
	public dialog: MatDialog, 
	public CommunityService:CommunityService, 
	public UserProfileService:UserProfileService,
	private _location: Location) { }

	public isAdmissionsLoading:boolean = true;
	public isPendingAdmissionsLoading:boolean = true;

  public anchors:Array<Anchor> = new Array();


  ngOnInit(): void {
    this.communityId = Number.parseInt(this.route.snapshot.paramMap.get("id") || "-1");

    this.CommunityService.getCommunityDetails(this.communityId!, true).subscribe(
      (res:any) => {
        this.CommunityModel = res as CommunityModel;
        this.CommunityModel!.admissionsUsers = new Array();
		this.CommunityModel!.pendingAdmissionsUsers	 = new Array();

        for (const admission of this.CommunityModel.admissions!) {
          this.UserProfileService.getProfile(admission.userId!).subscribe(
            (res:any) => { 
              let personne = res as UserProfile;
              personne.role = admission.roles;

              this.CommunityModel?.admissionsUsers?.push(personne);
            }
          )
        }
		this.isAdmissionsLoading = false;

		for (const admission of this.CommunityModel.pendingAdmissions!) {
			this.UserProfileService.getProfile(admission.userId!).subscribe(
			  (res:any) => {
				let personne = res as UserProfile;
				personne.role = admission.roles;
  
				this.CommunityModel?.pendingAdmissionsUsers?.push(personne);
			  }
			)
		}
		this.isPendingAdmissionsLoading = false;
      },
      err => {
        console.log(err);
      });

    this.CommunityService.getAnchorsByCommunity(this.communityId!).subscribe(
      (res:any) => {
        console.log(res);
        let result:MultiData<Anchor> = res;
        console.log(result);
        this.anchors = result.data!;
      },
      err => {
        console.log(err);
      }
    )
    //alert(communityId);
  }

  onBackClick()
  {
	  this._location.back();
  }

  onClick()
  {
    this.dialog.closeAll();
    this.dialog.open(InviteUserDialogComponent,{
      data: this.communityId,
    });
  }

}
