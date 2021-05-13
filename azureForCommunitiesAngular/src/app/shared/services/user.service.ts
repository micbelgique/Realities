import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Anchor } from '../models/anchor-model';
import { MultiData } from '../models/api-responses/multi-data.model';
import { TokenHolder } from '../models/tokenHolder.model';
import { UserProfile } from '../models/user-profile.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }


  getTokenHolder():TokenHolder
  {
    let tokenstring = localStorage.tkh;
    let tkh : TokenHolder = tokenstring === undefined ? new TokenHolder() : JSON.parse(tokenstring)	;
    return tkh;
  }

  getProfiles(ids:Array<string>):Promise<any>
  {
    let profiles = new Array<Promise<UserProfile>>();

    for (const id of ids) {
      profiles.push(this.getProfile(id));
    }

    return Promise.all(profiles);
  }

  getProfile(id:string):Promise<UserProfile>
  {
    let promise = new Promise<UserProfile>((resolve, reject) => {
      this.http.get(`${environment.apiUrl}/api/UsersAPI/${id}`).toPromise()
      .then(res => {
        let user:UserProfile = res;
        resolve(user);
      },
      err => {
        reject(err);
      })
    });

    return promise;
  }

  async isUserLoggedIn(): Promise<boolean>
  {
    let promise:Promise<boolean> = new Promise((resolve, reject) => {
		let tokenHolder:TokenHolder = this.getTokenHolder();
		if(tokenHolder.id === undefined)
		resolve(false);

		this.http.get(environment.apiUrl+"/api/Authentication")
		.toPromise()
		.then(
			res => {
			resolve(true);
			},
			err => {
			resolve(false);
			}
		)
    })

    return promise;
  }

  async getUserAnchors(id:string):Promise<Array<Anchor>>
  {
    let promise:Promise<Array<Anchor>> = new Promise((resolve, reject) => {
      this.http.get(environment.apiUrl+"/api/AnchorsAPI/user?UserId="+id).toPromise()
      .then(
        res => {
          let result:MultiData<Anchor> =  res;
          resolve(result.data!);
        },
        err => {
          reject();
        }
      )
    });

    return promise;
  }

  async updateUser(user:UserProfile):Promise<void>
  {	  
	let promise:Promise<void> = new Promise((resolve, reject) => {
		this.http.patch(environment.apiUrl + "/api/UsersAPI", user).toPromise()
		.then(
			res => {
				console.log(res);
				resolve();
			},
			err => {
				console.log(err);
				reject(err);
			}
		)
		
	});

	return promise;
  }


}
