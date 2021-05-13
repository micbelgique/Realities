import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {

  public user?:UserProfile = new UserProfile();

  constructor(public profileService:UserProfileService,private userService:UserService,private router:Router) { }

  ngOnInit(): void {
    this.userService.isUserLoggedIn().then(isUserLogged => {
      let tkh:TokenHolder = this.userService.getTokenHolder();
      this.profileService.getProfile(tkh.id).subscribe(
        (res: any) => {
          this.user = res;
          console.log(res);
        }
      )
    }).catch(err => {
      this.router.navigate(['/']);
    })

    
  }

  onSubmit(event:Event)
  {
    event.preventDefault();
    
    this.userService.updateUser(this.user!).then(res => {
      this.router.navigate(['/']);
    }).catch(
      err => {
        
      }
    )
  }

  onCancel()
  {
    this.router.navigate(['/']);
  }

}
