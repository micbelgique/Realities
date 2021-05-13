import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorModel } from 'src/app/shared/models/api-responses/error-model';
import { RegisterModel } from 'src/app/shared/models/register.model';
import { TokenHolder } from 'src/app/shared/models/tokenHolder.model';
import { LoginServiceService } from 'src/app/shared/services/login-service.service';
import { RegisterService } from 'src/app/shared/services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() onActionChanged: EventEmitter<string> = new EventEmitter();

  public RegisterService:RegisterService;
  public LoginService:LoginServiceService;

  constructor(RegisterService:RegisterService, LoginService:LoginServiceService, private router: Router) 
  {
    this.LoginService = LoginService;
    this.RegisterService = RegisterService;
  }

  onActionChangedClicked(e:Event)
  {
    e.preventDefault();
    this.onActionChanged.emit("login");
  } 

  getModel(): RegisterModel
  {
    return this.RegisterService.getModel();
  }

  onSubmit(e:Event)
  {
    e.preventDefault();
    this.RegisterService.sendRequest().subscribe(
      (res:any) => {
        console.log(res);
        this.LoginService.loginModel.email = this.RegisterService.RegisterModel.email || '';
        this.LoginService.loginModel.password = this.RegisterService.RegisterModel.password || '';

        this.LoginService.sendLogin().subscribe(
          (res:any) =>{
            let tkh:TokenHolder = res;

            localStorage.tkh = JSON.stringify(tkh);

            this.RegisterService.errorMessage = "";
            this.RegisterService.succesMessage = `Welcome ${tkh.id}`;
            //location.href = `unity-app://Login?token=${tkh.token}&userId=${tkh.id}`;
            location.reload(); 
          }
        )

      },
      err => {
        let res:ErrorModel = err.error;
        this.RegisterService.errorMessage = res.message;
        this.RegisterService.succesMessage = "";

      }
    );
  }

  ngOnInit(): void {
  }

}
