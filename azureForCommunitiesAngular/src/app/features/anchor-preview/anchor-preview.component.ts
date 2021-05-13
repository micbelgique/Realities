import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { FullAnchor } from 'src/app/shared/models/full-anchor.model';
import { Interaction } from 'src/app/shared/models/interaction.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { AnchorDetailsService } from 'src/app/shared/services/anchor-details.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-anchor-preview',
  templateUrl: './anchor-preview.component.html',
  styleUrls: ['./anchor-preview.component.css']
})
export class AnchorPreviewComponent implements OnInit {

  public id:string = "";
  public isOwner:boolean = false;
  public actualOwner:string = "";
  public isLoggedIn:boolean = false;
  public hasBeenDeleted:boolean = false;
  public anchor?:FullAnchor = undefined;

  public content:string="";

  constructor(private route: ActivatedRoute, private anchorService:AnchorDetailsService, private userService:UserService) { }

  ngOnInit(): void {
    let id = this.route.snapshot.paramMap.get("id") || 'none';
    this.id = id;
    
    this.anchorService.getAnchorDetails(this.id).subscribe(
      res => {
        if(res == null)
        {
          this.hasBeenDeleted = true;
          return;
        }
        this.anchor = res;

        let usersID:Array<string> = new Array();
        for (const interaction of this.anchor.interactions!) {
          if(!usersID.includes(interaction.userId!))
            usersID.push(interaction.userId!);
        }
        this.userService.getProfiles(usersID).then(
          res => 
            {
              let users:Array<UserProfile> = res;
              for (let interaction of this.anchor?.interactions!) {
                interaction.userName = users.find(x => x.id == interaction.userId!)?.nickName;
              } 
            }
        )

        let userId = this.userService.getTokenHolder().id;
        this.actualOwner = userId;
        this.isOwner = (userId == this.anchor.userId);
      },
      err => console.log(err)
    )

    this.userService.isUserLoggedIn().then(isUserLogged => this.isLoggedIn=isUserLogged);

  }

  onDetails()
  {
    location.href = `unity-app://Details?anchor=${this.id}`;
  }

  onDelete()
  {
    location.href = `unity-app://Delete?anchor=${this.id}`;
    this.anchorService.deleteAnchor(this.anchor!).then(res => this.hasBeenDeleted = true).catch(err => console.log(err));
  }

  onCancel()
  {
    location.href = `unity-app://Cancel`;
  }

  onClick()
  {
    let interaction:Interaction = new Interaction();
    interaction.anchorIdentifier = this.anchor!.identifier!;
    interaction.message = this.content;
    interaction.userId = this.userService.getTokenHolder().id;
    this.anchorService.postAnchorInteraction(interaction).then(res => {
      this.anchor?.interactions?.push(res);
      this.content = "";
    })
    
  }

}
