import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { MultiData } from '../models/api-responses/multi-data.model';
import { ReportCreate } from '../models/reports/report-crate.model';
import { Report } from '../models/reports/report.model';

@Injectable({
  providedIn: 'root'
})
export class ReportsService {

  constructor(private http:HttpClient) { }

  report(report:ReportCreate):Promise<void>
  {
    let promise = new Promise<void>((resolve, reject) => {
      this.http.post(environment.apiUrl + "/api/ReportsAPI", report).toPromise()
      .then(
        res => {
          resolve();
        },
        err => {
          reject(err);
        }
      )
    });

    return promise;
  }

  getAllReports():Promise<Array<Report>>
  {
    let promise = new Promise<Array<Report>>((resolve, reject) => {
      this.http.get(environment.apiUrl + "/api/ReportsAPI").toPromise()
      .then(
        res => {
          let response = <MultiData<Report>>res;
          let reports = <Array<Report>>response.data;
          resolve(reports);
        },
        err => {
          reject(err);
        }
      )
    });

  return promise;
  }

  validateReport(report:Report):Promise<void>
  {
    let promise = new Promise<void>((resolve, reject) => {

      const options = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
        }),
        body: report,
      };

      this.http.delete(environment.apiUrl+"/api/ReportsAPI/resolve", options).toPromise()
      .then(
        res => {
          resolve();
        },
        err => {
          reject();
        }
      )
    })

    return promise;

  }

  cancelReport(report:Report):Promise<void>
  {
    let promise = new Promise<void>((resolve, reject) => {

      this.http.post(environment.apiUrl+"/api/ReportsAPI/resolve", report).toPromise()
      .then(
        res => {
          resolve();
        },
        err => {
          reject();
        }
      )
    })

    return promise;

  }
}
