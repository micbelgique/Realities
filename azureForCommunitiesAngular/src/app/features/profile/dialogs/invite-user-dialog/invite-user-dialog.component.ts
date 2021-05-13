import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AdmissionRequest } from 'src/app/shared/models/api-requests/admission-request.model';
import { CommunityService } from 'src/app/shared/services/community.service';

@Component({
  selector: 'app-invite-user-dialog',
  templateUrl: './invite-user-dialog.component.html',
  styleUrls: ['./invite-user-dialog.component.css']
})
export class InviteUserDialogComponent implements OnInit {

  public AdmissionRequest!:AdmissionRequest;
  public errorMessage?:string;

  constructor(
    public dialogRef: MatDialogRef<InviteUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data:number, 
    public CommunityService:CommunityService) 
  {
    this.AdmissionRequest = new AdmissionRequest();
    this.AdmissionRequest.communityId = data;
  }

  ngOnInit(): void {
    
  }

  onSubmit(e:Event)
  {
    e.preventDefault();
    this.CommunityService.inviteUser(this.AdmissionRequest).subscribe(
      (res:any) => {
        alert("ok, user invited");
        this.dialogRef.close();
      },
      err => {
        console.log(err);
        this.errorMessage = `user ${this.AdmissionRequest.userEmail} either does not exist or is already invited here`;
      }
    )


  }

}
