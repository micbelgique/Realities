import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RegisterModel } from '../models/register.model';

@Injectable({
  providedIn: 'root'
})

export class RegisterService {
  public errorMessage?: string;
  public succesMessage?: string;

  public RegisterModel: RegisterModel;
  private http : HttpClient;
  

  constructor(http: HttpClient) 
  { 
    this.http = http;
    this.RegisterModel = new RegisterModel();
  }

  getModel(): RegisterModel
  {
    return this.RegisterModel;
  }

  sendRequest()
  {
    return this.http.post(`${environment.apiUrl}/api/UsersAPI`, this.RegisterModel);
  }
}
