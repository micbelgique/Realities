import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Anchor } from '../models/anchor-model';
import { Interaction } from '../models/interaction.model';

@Injectable({
  providedIn: 'root'
})
export class AnchorDetailsService {
  private http : HttpClient;

  constructor(http:HttpClient) 
  {
      this.http = http;
  }

  getAnchorDetails(anchor:string)
  {
    return this.http.get(`${environment.apiUrl}/api/AnchorsAPI/${anchor}`);
  }

  postAnchorInteraction(interaction:Interaction):Promise<Interaction>
  {
    let promise= new Promise<Interaction>((resolve,reject) => {
      this.http.post(`${environment.apiUrl}/api/InteractionsAPI`, interaction).toPromise()
      .then(res => {
        let result:Interaction = res;
        resolve(result);
      },
      err => {
        reject(err);
      })
    });

    return promise;
  }

  deleteAnchor(anchor:Anchor):Promise<void>
  {

    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: anchor,
    };

    let promise = new Promise<void>((resolve, reject) => {
      this.http.delete(environment.apiUrl+"/api/AnchorsAPI", options).toPromise()
      .then(
        res => resolve(),
        err => reject()
      )
    });

    return promise;
  }

}
