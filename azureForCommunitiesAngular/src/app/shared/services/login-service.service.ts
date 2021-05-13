import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginModel } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})

export class LoginServiceService {
  public errorMessage?: string;
  public succesMessage? :string;

  public loginModel!: LoginModel;
  private http : HttpClient;

  constructor(http : HttpClient) { 
    this.http = http;
    this.loginModel = new LoginModel();
  }

  sendLogin()
  {
    return this.http.post(`${environment.apiUrl}/api/Authentication`, this.loginModel);
  }
}
