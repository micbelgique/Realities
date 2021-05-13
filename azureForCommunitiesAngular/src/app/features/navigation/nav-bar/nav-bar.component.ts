import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  @Input() previousEnabled:boolean = false;
  constructor(private userService:UserService, private router: Router) { }

  ngOnInit(): void {
  }
  
  onClick()
  {
    let tkh : TokenHolder = this.userService.getTokenHolder();
    location.href = `unity-app://Login?token=${tkh.token}&userId=${tkh.id}`;
  }

  onARClick()
  {
    let tkh : TokenHolder = this.userService.getTokenHolder();
    location.href = `unity-app://Login?token=${tkh.token}&userId=${tkh.id}`;
  }

  onPreviousClick()
  {
    if(window.history.state.navigationId != 1)
    {
      this.router.navigate(["/"]);
    }
    else
    {
      this.onARClick();
      this.router.navigate(["/"]);
    }
  }

}
