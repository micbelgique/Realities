import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel } from 'src/app/shared/models/login.model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { LoginServiceService } from 'src/app/shared/services/login-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  
  @Output() onActionChanged: EventEmitter<string> = new EventEmitter();
  

  public visibility:boolean = false;

  constructor(public LoginService: LoginServiceService, private router: Router) 
  { 

  }

  onActionChangedClicked(e:Event)
  {
    e.preventDefault();
    this.onActionChanged.emit("register");
  } 

  getModel() : LoginModel
  {
    return this.LoginService.loginModel; 
  }

  onSubmit(e:Event)
  {
    e.preventDefault();
    this.LoginService.sendLogin().subscribe(
      (res:any) => {
        let tkh:TokenHolder = res;

        localStorage.tkh = JSON.stringify(tkh);

        this.LoginService.errorMessage = "";
        this.LoginService.succesMessage = `Welcome ${tkh.id}`;
        location.reload(); 
      },
      err => {
        console.log(err);
        switch(err.status)
        {
          case 401: 
            this.LoginService.errorMessage = "Your credentials don't match";
            break;

            default:  this.LoginService.errorMessage = "[unkown] " + err;
        }
        
        this.LoginService.succesMessage = "";
      }
    );
  }

  ngOnInit(): void 
  {

  }

  changeVisibility() 
  {
    this.visibility = !this.visibility;
  }

}
