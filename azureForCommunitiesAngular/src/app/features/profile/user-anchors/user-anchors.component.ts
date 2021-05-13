import { Component, Input, OnInit } from '@angular/core';
import { Anchor } from 'src/app/shared/models/anchor-model';
import { MultiData } from 'src/app/shared/models/api-responses/multi-data.model';
import { Placement } from 'src/app/shared/models/placement.model';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';

@Component({
  selector: 'app-user-anchors',
  templateUrl: './user-anchors.component.html',
  styleUrls: ['./user-anchors.component.css']
})
export class UserAnchorsComponent implements OnInit {

  @Input() anchors?:Array<Placement>;

  constructor(public UserProfileService:UserProfileService) { }

  ngOnInit(): void {
    
  }

  getModel():Array<Placement>
  {
    return this.anchors || new Array<Placement>();
  }
}
