import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";

import { MatSliderModule } from '@angular/material/slider';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatTabsModule} from '@angular/material/tabs';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatBadgeModule} from '@angular/material/badge';
import {MatDividerModule} from '@angular/material/divider';
import {MatIconModule} from '@angular/material/icon';
import {MatListModule} from '@angular/material/list';
import {MatSelectModule} from '@angular/material/select';
import {MatDialogModule} from '@angular/material/dialog';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSidenavModule} from '@angular/material/sidenav';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './features/user-authentication/login/login.component';
import { LoginServiceService } from './shared/services/login-service.service';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RegisterComponent } from './features/user-authentication/register/register.component';
import { RegisterService } from './shared/services/register.service';
import { UserProfileComponent } from './features/profile/user-profile/user-profile.component';
import { UserProfileService } from './shared/services/user-profile.service';
import { GoogleMapsModule } from '@angular/google-maps';
import { AnchorsMapComponent } from './features/anchors-map/anchors-map.component';
import { AnchorsMapService } from './shared/services/anchors-map.service';
import { TokenHolder } from './shared/models/tokenHolder.model';
import { CommunityProfileComponent } from './features/profile/community-profile/community-profile.component';
import { ProfileComponent } from './features/profile/profile/profile.component';
import { UserAnchorsComponent } from './features/profile/user-anchors/user-anchors.component';
import { HeatMapComponent } from './features/heat-map/heat-map.component';
import { AnchorDetailsComponent } from './features/anchor-details/anchor-details.component';
import { NewCommunityDialogComponent } from './features/profile/dialogs/new-community-dialog/new-community-dialog.component';
import { CreateCommunityComponent } from './features/profile/create-community/create-community.component';
import { CommunityProfileDetailsComponent } from './features/profile/community-profile-details/community-profile-details.component';
import { InviteUserDialogComponent } from './features/profile/dialogs/invite-user-dialog/invite-user-dialog.component';
import { CommunityService } from './shared/services/community.service';
import { ShopContainerComponent } from './features/shop/shop-container/shop-container.component';
import { ShopCategoryComponent } from './features/shop/shop-category/shop-category.component';
import { ShopVisibilityComponent } from './features/shop/shop-visibility/shop-visibility.component';
import { ShopDataCompletionComponent } from './features/shop/shop-data-completion/shop-data-completion.component';
import { UserTutorialComponent } from './features/guides/user-tutorial/user-tutorial.component';
import { AnchorsDiscoveryComponent } from './features/profile/anchors-discovery/anchors-discovery.component';
import { AnchorTemplateComponent } from './features/templates/anchor-template/anchor-template/anchor-template.component';
import { AuthenticationComponent } from './features/user-authentication/authentication/authentication.component';
import { UploadImageTestComponent } from './features/upload-image-test/upload-image-test.component';
import { EditProfileComponent } from './features/profile/edit-profile/edit-profile.component';
import { TermsAndConditionsComponent } from './features/guides/terms-and-conditions/terms-and-conditions.component';
import { ReportAnchorDialogComponent } from './features/dialogs/report-anchor-dialog/report-anchor-dialog.component';
import { ReportsService } from './shared/services/reports.service';
import { AnchorPreviewComponent } from './features/anchor-preview/anchor-preview.component';
import { ReportsPannelComponent } from './features/admin/reports-pannel/reports-pannel.component';
import { NavBarComponent } from './features/navigation/nav-bar/nav-bar.component';
import { InteractionChatComponent } from './features/interactions/interaction-chat/interaction-chat.component';

export function getSessionToken()
{
  let tkh : TokenHolder = localStorage.tkh != null ? JSON.parse(localStorage.tkh) : new TokenHolder("test");
  return tkh.token;
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserProfileComponent,
    AnchorsMapComponent,
    CommunityProfileComponent,
    ProfileComponent,
    UserAnchorsComponent,
    HeatMapComponent,
    AnchorDetailsComponent,
    NewCommunityDialogComponent,
    CreateCommunityComponent,
    CommunityProfileDetailsComponent,
    InviteUserDialogComponent,
    ShopContainerComponent,
    ShopCategoryComponent,
    ShopVisibilityComponent,
    ShopDataCompletionComponent,
    UserTutorialComponent,
    AnchorsDiscoveryComponent,
    AnchorTemplateComponent,
    AuthenticationComponent,
    UploadImageTestComponent,
    EditProfileComponent,
    TermsAndConditionsComponent,
    ReportAnchorDialogComponent,
    AnchorPreviewComponent,
    ReportsPannelComponent,
    NavBarComponent,
    InteractionChatComponent
  ],
  imports: [
    MatTabsModule,
    MatSliderModule,
    MatExpansionModule,
    MatBadgeModule,
    MatSidenavModule,
    MatDialogModule,
    MatProgressBarModule,
    MatSelectModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatInputModule,
    MatDividerModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    GoogleMapsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: getSessionToken,
        allowedDomains: ["localhost:5000", "85.201.245.13:5000", "85.201.245.13:44391", "hiw-communities.azurewebsites.net"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    LoginServiceService,
    UserProfileService,
    RegisterService,
    AnchorsMapService,
    CommunityService,
    ReportsService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
