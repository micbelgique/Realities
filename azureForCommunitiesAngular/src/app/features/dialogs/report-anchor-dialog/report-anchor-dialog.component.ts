import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReportCreate } from 'src/app/shared/models/reports/report-crate.model';
import { ReportsService } from 'src/app/shared/services/reports.service';

@Component({
  selector: 'app-report-anchor-dialog',
  templateUrl: './report-anchor-dialog.component.html',
  styleUrls: ['./report-anchor-dialog.component.css']
})
export class ReportAnchorDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ReportAnchorDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public createReportModel:ReportCreate, private reportService:ReportsService) { }

  ngOnInit(): void {
  }

  onReport()
  {
    this.reportService.report(this.createReportModel).then(res => this.dialogRef.close()).catch(err => console.log(err));
  }

}

