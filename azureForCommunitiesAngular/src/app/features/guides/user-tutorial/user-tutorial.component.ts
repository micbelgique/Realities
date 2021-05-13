import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-user-tutorial',
  templateUrl: './user-tutorial.component.html',
  styleUrls: ['./user-tutorial.component.css']
})
export class UserTutorialComponent implements OnInit {

  constructor(private router: Router, private userService:UserService) { }

  ngOnInit(): void {
  }

  onClick()
  {
    localStorage.tutorialChecked = true;
    let tkh : TokenHolder = this.userService.getTokenHolder();
    location.href = `unity-app://Login?token=&userId=${tkh.id}`;
    this.router.navigate(['/']);
  }

}
