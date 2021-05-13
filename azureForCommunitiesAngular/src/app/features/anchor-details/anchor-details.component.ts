import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, NavigationEnd, Router, RoutesRecognized } from '@angular/router';
import { FullAnchor } from 'src/app/shared/models/full-anchor.model';
import { Interaction } from 'src/app/shared/models/interaction.model';
import { ReportCreate } from 'src/app/shared/models/reports/report-crate.model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { AnchorDetailsService } from 'src/app/shared/services/anchor-details.service';
import { UserService } from 'src/app/shared/services/user.service';
import { ReportAnchorDialogComponent } from '../dialogs/report-anchor-dialog/report-anchor-dialog.component';
import { filter, pairwise } from 'rxjs/operators';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-anchor-details',
  templateUrl: './anchor-details.component.html',
  styleUrls: ['./anchor-details.component.css']
})
export class AnchorDetailsComponent implements OnInit {

  public Anchor?:FullAnchor;
  public UserProfile?:UserProfile;
  public actualUserId:string = "";
  public isUserLoggedIn:boolean = false;

  public content:string="";

  constructor(private route: ActivatedRoute, public AnchorDetailsService:AnchorDetailsService, private userService:UserService, public dialog: MatDialog) 
  {
  }

  ngOnInit(): void {


    this.userService.isUserLoggedIn().then(res => this.isUserLoggedIn = res);
    this.actualUserId = this.userService.getTokenHolder().id;

    let id = this.route.snapshot.paramMap.get("id") || 'none';
    this.AnchorDetailsService.getAnchorDetails(id!).subscribe(
      (res:any) => {
        console.log(res);
        this.Anchor = res;
        let usersID:Array<string> = new Array();
        for (const interaction of this.Anchor!.interactions!) {
          if(!usersID.includes(interaction.userId!))
            usersID.push(interaction.userId!);
        }
        this.userService.getProfiles(usersID).then(
          res => 
            {
              let users:Array<UserProfile> = res;
              for (let interaction of this.Anchor?.interactions!) {
                interaction.userName = users.find(x => x.id == interaction.userId!)?.nickName;
              } 
            }
        )
        this.userService.getProfile(this.Anchor?.userId!).then(
          res => this.UserProfile = res
        )
      },
      err => {
          alert("not found");
      }
    )
  }

  onReportClick()
  {
    let report = new ReportCreate();
    report.anchorIdentifier = this.Anchor?.identifier!;
    report.userId = this.userService.getTokenHolder().id;
    this.dialog.open(ReportAnchorDialogComponent,{
      data: report,
    });
  }

}
