import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css']
})
export class AuthenticationComponent implements OnInit {

  public action:string;

  constructor(private route: ActivatedRoute) { 
    this.action = "login";
  }

  ngOnInit(): void {
    this.route.queryParams
      .subscribe(params => {
        console.log(params); // { orderby: "price" }
        this.action = params.action || 'login';
      }
    );
  }

  onVisibilityChanged(action:string)
  {
    this.action = action;
  }

}
