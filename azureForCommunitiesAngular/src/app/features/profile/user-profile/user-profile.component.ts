import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  @Input() user?:UserProfile;
  @Input() anchors?:Array<Anchor>;

  constructor(private router:Router) 
  {

  }

  ngOnInit(): void {
    
  }

  getModel(): UserProfile
  {
    return this.user || new UserProfile();
  }

  editClick()
  {
    this.router.navigate(['/editProfile'])
  }

  onClickLogout()
  {
    localStorage.removeItem("tkh");
    location.href = `unity-app://Logout`;
    location.reload();
  }


}
