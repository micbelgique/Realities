import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Anchor } from '../models/anchor-model';
import { MultiData } from '../models/api-responses/multi-data.model';

@Injectable({
  providedIn: 'root'
})
export class AnchorsMapService {

  private http : HttpClient;

  constructor(http:HttpClient) 
  {
    this.http = http;
  }

  getCommunities()
  {
    return this.http.get(`${environment.apiUrl}/api/CommunitiesAPI`);
  }

  getAnchors():Promise<Array<Anchor>>
  {
    let promise = new Promise<Array<Anchor>>((resolve, reject) => {
      this.http.get(`${environment.apiUrl}/api/AnchorsAPI`)
      .toPromise()
      .then(
        res => {
          let data:MultiData<Anchor> = res;
          resolve(data.data!);
        },
        err => {
          reject(err);
        }
      )
    }); 

    return promise;
  }


  getAnchorsForUser(userId:string):Promise<Array<Anchor>>
  {
    let promise = new Promise<Array<Anchor>>((resolve, reject) => {
      this.http.get(`${environment.apiUrl}/api/AnchorsAPI/forUser?UserId=${userId}`)
      .toPromise()
      .then(
        res => {
          let data:MultiData<Anchor> = res;
          resolve(data.data!);
        },
        err => {
          reject(err);
        }
      )
    }); 

    return promise;
  }

  getPublicAnchors(): Promise<Array<Anchor>>
  {
    let promise = new Promise<Array<Anchor>>((resolve, reject) => {
      this.http.get(`${environment.apiUrl}/api/AnchorsAPI`)
        .toPromise()
        .then(
          res => {
            let data:MultiData<Anchor> = res;
            resolve(data.data!);
          },
          err => {
            reject(err);
          })
    });
    
    return promise;
  }

  getNearAnchors(lat:number, lng:number, userid?:string): Promise<Array<Anchor>>
  {
    let promise = new Promise<Array<Anchor>>((resolve, reject) => {
      this.http.get(environment.apiUrl+"/api/AnchorsAPI/near?Longitude="+lng+"&Latitude="+lat+"&UserId="+userid)
        .toPromise()
        .then(
          res => {
            let data:MultiData<Anchor> = res;
            resolve(data.data!);
          },
          err => {
            reject(err);
          })
    });
    
    return promise;
  }
}
