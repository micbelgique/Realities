import { Component, OnInit } from '@angular/core';
import { Report } from 'src/app/shared/models/reports/report.model';
import { ReportsService } from 'src/app/shared/services/reports.service';

@Component({
  selector: 'app-reports-pannel',
  templateUrl: './reports-pannel.component.html',
  styleUrls: ['./reports-pannel.component.css']
})
export class ReportsPannelComponent implements OnInit {


  public reports:Array<Report> = new Array();

  constructor(private reportService:ReportsService) { }

  ngOnInit(): void {
    this.reportService.getAllReports().then(
      res => this.reports = res
    ).catch(err => console.log(err));
  }

  onValidateReportClick(report:Report)
  {
    console.log(report);

    this.reportService.validateReport(report).
    then(
      res => {
        this.reportService.getAllReports().then(
          res => this.reports = res
        ).catch(err => console.log(err));
      }
    )
  }

  onRefuseReportClick(report:Report)
  {
    console.log(report);
    this.reportService.cancelReport(report).
    then(
      res => {
        this.reportService.getAllReports().then(
          res => this.reports = res
        ).catch(err => console.log(err));
      }
    )
  }

}
