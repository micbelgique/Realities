import { Component, Input, OnInit } from '@angular/core';
import { Interaction } from 'src/app/shared/models/interaction.model';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-interaction-chat',
  templateUrl: './interaction-chat.component.html',
  styleUrls: ['./interaction-chat.component.css']
})
export class InteractionChatComponent implements OnInit {

  @Input() interactions:Array<Interaction> = new Array();
  public user:string;

  constructor(private userService:UserService) 
  {
    this.user = this.userService.getTokenHolder().id;
  }

  ngOnInit(): void {
  }

}
