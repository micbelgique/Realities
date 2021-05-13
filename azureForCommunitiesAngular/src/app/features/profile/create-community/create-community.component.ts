import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommunityModel } from 'src/app/shared/models/community-model';
import { EpiCenterModel } from 'src/app/shared/models/epiCenter-model';
import { RegisterCommunityService } from 'src/app/shared/services/register-community.service';

@Component({
  selector: 'app-create-community',
  templateUrl: './create-community.component.html',
  styleUrls: ['./create-community.component.css']
})
export class CreateCommunityComponent implements OnInit {

  public CommunityModel?:CommunityModel;

  constructor(public createCommunityService:RegisterCommunityService, private router: Router) { }

  ngOnInit(): void {
    this.CommunityModel = new CommunityModel();
  }

  onSubmit(e:Event)
  {
    e.preventDefault();
    this.CommunityModel!.isLocated = false;
    this.CommunityModel!.address = 'none';
    let epiCenter:EpiCenterModel = new EpiCenterModel();

    epiCenter.latitude = 0;
    epiCenter.longitude = 0;
    epiCenter.srid = 4326;
    epiCenter.radius = 10;

    this.CommunityModel!.epiCenter = epiCenter; 

    this.createCommunityService.sendCommunity(this.CommunityModel!).subscribe(
      (res:any) => {
        this.router.navigate(['profile']);
      },
      err => {
        alert("error: " + err);
      }
    )
  }

  

}
