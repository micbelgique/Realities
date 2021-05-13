import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AnchorDetailsComponent } from './features/anchor-details/anchor-details.component';
import { AnchorPreviewComponent } from './features/anchor-preview/anchor-preview.component';
import { AnchorsMapComponent } from './features/anchors-map/anchors-map.component';
import { TermsAndConditionsComponent } from './features/guides/terms-and-conditions/terms-and-conditions.component';
import { UserTutorialComponent } from './features/guides/user-tutorial/user-tutorial.component';
import { HeatMapComponent } from './features/heat-map/heat-map.component';
import { CommunityProfileDetailsComponent } from './features/profile/community-profile-details/community-profile-details.component';
import { CreateCommunityComponent } from './features/profile/create-community/create-community.component';
import { EditProfileComponent } from './features/profile/edit-profile/edit-profile.component';
import { ProfileComponent } from './features/profile/profile/profile.component';
import { ShopContainerComponent } from './features/shop/shop-container/shop-container.component';
import { UploadImageTestComponent } from './features/upload-image-test/upload-image-test.component';

const routes: Routes = 
[
  {path:'', redirectTo:'profile', pathMatch:'full'},
  {path:'profile', component:ProfileComponent},
  {path:'map', component:AnchorsMapComponent},
  {path:'heat-map', component:HeatMapComponent},
  {path:'anchorDetail/:id', component:AnchorDetailsComponent},
  {path:'createCommunity', component:CreateCommunityComponent},
  {path:'community/:id', component:CommunityProfileDetailsComponent},
  {path:'shop', component:ShopContainerComponent},
  {path:'tutorial', component:UserTutorialComponent},
  {path:'image', component:UploadImageTestComponent},
  {path:'editProfile', component:EditProfileComponent},
  {path:'anchorPreview/:id', component:AnchorPreviewComponent},
  {path:'terms', component:TermsAndConditionsComponent}
]
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
