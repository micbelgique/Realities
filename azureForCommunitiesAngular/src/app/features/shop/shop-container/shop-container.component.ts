import { EventEmitter, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { CommunityModel } from 'src/app/shared/models/community-model';
import { ShopCategory } from 'src/app/shared/models/shop/shop-category.model';
import { ShopContainer } from 'src/app/shared/models/shop/shop-container.model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserProfile } from 'src/app/shared/models/user-profile.model';
import { CommunityService } from 'src/app/shared/services/community.service';
import { UserProfileService } from 'src/app/shared/services/user-profile.service';
import * as countrie from  "src/app/_files/dummy.json"
import { DataCompletionParam, DataCompletionType } from '../shop-data-completion/shop-data-completion.component';
import { ShopStep } from '../shop-step';
import { VisibilityParam } from '../shop-visibility/shop-visibility.component';

@Component({
  selector: 'app-shop-container',
  templateUrl: './shop-container.component.html',
  styleUrls: ['./shop-container.component.css']
})
export class ShopContainerComponent implements OnInit {

  public step:ShopStep = ShopStep.ModelSelection;
  public categories?:Array<ShopCategory> = new Array();

  private model?:string;
  private data?:DataCompletionParam;
  private visibilty?:VisibilityParam;

  public dataType:DataCompletionType = DataCompletionType.image;

  private userProfile?:UserProfile;

  public userCommunities?:Array<CommunityModel> = new Array();

  constructor(private userService:UserProfileService, private communityService:CommunityService) { }

  ngOnInit(): void {
	this.categories = countrie.categories;
	console.log(this.categories)

	let tkh:TokenHolder = JSON.parse(localStorage.tkh || "{}");
	this.userService.getProfile(tkh.id).subscribe(
	  (res:any) => {
		this.userProfile = res;

		for (const admission of this.userProfile?.admissions!) {
		  this.communityService.getCommunityDetails(admission.community!, false).subscribe(
			(res:any) => {
			  this.userCommunities?.push(res);
			},
			err => {

			}
		  )
		}

	  },
	  err => {
		
	  }
	)

  }
  
  selectItem(newItem: string) {
	//alert('wired: ' + newItem);
	this.model = newItem;

	switch (newItem) {
		case "pannel":
			this.step = ShopStep.DataCompletion;
			this.dataType = DataCompletionType.pannel;
			break;
		
		case "image":
			this.step = ShopStep.DataCompletion;
			this.dataType = DataCompletionType.image;
			break;
	
		default:
			this.step = ShopStep.ModelVisibility;
			break;
	}	  
  }

  selectVisibilty(value:VisibilityParam)
  {
	this.visibilty = value;
	this.step = ShopStep.ModelSelection;
	if(this.data)
	  this.sendFullToUnity();
	else
	  this.sendToUnity();
  }

  endDataCompletion(value:DataCompletionParam)
  {
	this.data = value;
	this.step = ShopStep.ModelVisibility;
  }

  previousStep()
  {
	this.step = ShopStep.ModelSelection;
  }

  sendToUnity()
  {
	location.href = `unity-app://ModelSelected?model=${this.model || 'none'}&visibility=${this.visibilty?.visibilty}&community=${this.visibilty?.communityId}`;
  }

  sendFullToUnity()
  {
	  //console.log(encodeURI(`model=${this.model || 'none'}&visibility=${this.visibilty?.visibilty}&community=${this.visibilty?.communityId}&title=${this.data?.title}&content=${this.data?.text}&imageUrl=${this.data?.imageUrl}`));
	location.href = `unity-app://ModelSelected?model=${this.model || 'none'}&visibility=${this.visibilty?.visibilty}&community=${this.visibilty?.communityId}&title=${this.data?.title}&content=${this.data?.text}&imageUrl=${this.data?.imageUrl}`;
  }

}
