import { Component, OnInit } from '@angular/core';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { LocationModel } from 'src/app/shared/models/location.model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { AnchorsMapService } from 'src/app/shared/services/anchors-map.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-anchors-discovery',
  templateUrl: './anchors-discovery.component.html',
  styleUrls: ['./anchors-discovery.component.css']
})
export class AnchorsDiscoveryComponent implements OnInit {

  public anchorsNearYou:Array<Anchor> = new Array();
  public anchorsForYou:Array<Anchor> = new Array();
  public userPosition?:LocationModel;
  public isUserLocated:boolean = false;

  constructor(private AnchorService:AnchorsMapService, private userService:UserService) { }

  ngOnInit(): void {

    if(navigator.geolocation) {

      let tkh:TokenHolder = this.userService.getTokenHolder();

      navigator.geolocation.getCurrentPosition((position:GeolocationPosition) => {
        console.log(position);
        this.userPosition = new LocationModel(position.coords.latitude, position.coords.longitude);
        this.isUserLocated = true;

        this.AnchorService.getNearAnchors(position.coords.latitude, position.coords.longitude, tkh.id)
          .then((anchors) => {
            this.anchorsNearYou = anchors;
            let users:Array<string> = new Array();
            for (let anchor of this.anchorsNearYou) {
              if(!users.includes(anchor.userId!))
                users.push(anchor.userId!);
            } 

            this.userService.getProfiles(users)
            .then(res => 
            {
              let users:Array<UserProfile> = res;
              for (let anchor of this.anchorsNearYou) {
                anchor.userName = users.find(x => x.id == anchor.userId!)?.nickName;
              } 
            });


          })
          .catch(err => {console.log(err)});

        this.userService.isUserLoggedIn().then(isUserLogged => {
          if(isUserLogged)
          {
            this.AnchorService.getAnchorsForUser(tkh.id)
            .then((anchors) => {this.anchorsForYou = anchors; console.log(anchors)})
            .catch(err => {console.log(err)});
          }
          else
          {
            this.AnchorService.getAnchors().then(anchors => {this.anchorsForYou = anchors; console.log(anchors) });
          }
        });
      } , 
      err => {
        this.userPosition = new LocationModel(50.45857741638403, 3.9413667667414143);
        this.isUserLocated = false;
      })
    }
    else
    {
      console.log("geolocation not supported");
      this.userPosition = new LocationModel(50.45857741638403, 3.9413667667414143);
      this.isUserLocated = false;
    }

    
    
  }

}
