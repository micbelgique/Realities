import { Component, Input, OnInit } from '@angular/core';
import { Anchor } from 'src/app/shared/models/anchor-model';

@Component({
  selector: 'app-anchor-template',
  templateUrl: './anchor-template.component.html',
  styleUrls: ['./anchor-template.component.css']
})
export class AnchorTemplateComponent implements OnInit {

  @Input() anchor!:Anchor;

  constructor() { }

  ngOnInit(): void {
  }

}
