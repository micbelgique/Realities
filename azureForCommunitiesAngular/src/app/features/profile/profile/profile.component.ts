import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { MultiData } from 'src/app/shared/models/api-responses/multi-data.model';
import { CommunityModel } from 'src/app/shared/models/community-model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public users: Map<string, UserProfile> = new Map<string, UserProfile>();

  public communityModel?:CommunityModel;
  public userAnchors?:Array<Anchor>;

  public isUserLoggedIn:boolean = false;

  private _selectedIndex: number = 0;

  public get selectedIndex(): number {
    return this._selectedIndex;
  }

  public set selectedIndex(value: number) {
    this._selectedIndex = value;
    localStorage.profilIndex = value;
  }

  constructor(public profileService:UserProfileService,private userService:UserService, private router: Router) { }

  

    ngOnInit() {

      if(localStorage.profilIndex == undefined)
        this.selectedIndex = 0;   

      if(localStorage.tutorialChecked == undefined || localStorage.tutorialChecked == false)
      {
        this.router.navigate(['tutorial']);
      }

      this.userService.isUserLoggedIn().then(
        (isUserLoggedIn) => {
          this.isUserLoggedIn = isUserLoggedIn;
          console.log(isUserLoggedIn);

          if(isUserLoggedIn){
            let tokenHolder:TokenHolder = this.userService.getTokenHolder();
            this.getUserData(tokenHolder.id);
          }
          this.selectedIndex = localStorage.profilIndex;

      });

  }


  
  onRefresh()
  {
    location.reload();
  }

  getUserData(id:string)
  {
	this.profileService.getProfile(id).subscribe(
		(res:any) => {
			this.profileService.UserProfile = res;
			console.log(this.profileService.UserProfile);
      this.userService.getUserAnchors(this.profileService.UserProfile.id!)
        .then(res =>  {
          this.userAnchors = res;
          for(let anchor of this.userAnchors)
          {
            anchor.userName = this.profileService.UserProfile.nickName;
          }
        });
		},
			err => {
			console.debug(err);
		});

	console.debug(this.users);
  }

  getUsersFromAnchors(anchors:MultiData<Anchor>)
  {
    anchors?.data?.forEach(element => {
      if(!this.users.has(element.userId!))
      {
        this.profileService.getProfile(element.userId!).subscribe(
          (res:any) => {
            this.users.set(element.userId!,res);
          }
        );
      }
    });
  }
}
