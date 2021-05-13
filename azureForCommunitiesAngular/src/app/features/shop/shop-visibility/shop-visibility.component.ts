import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommunityModel } from 'src/app/shared/models/community-model';

@Component({
  selector: 'app-shop-visibility',
  templateUrl: './shop-visibility.component.html',
  styleUrls: ['./shop-visibility.component.css']
})
export class ShopVisibilityComponent implements OnInit {

  constructor() { }
  selectedVisibility = '0';
  selectedCommunity = '-1';

  @Input() userCommunities?:Array<CommunityModel>;

  @Output() onSelectVisibilty: EventEmitter<VisibilityParam> = new EventEmitter();
  @Output() onPrevious: EventEmitter<void> = new EventEmitter();

  endChoice(): void {
      let value:VisibilityParam = new VisibilityParam();
      value.visibilty = Number.parseInt(this.selectedVisibility);
      value.communityId = Number.parseInt(this.selectedCommunity);
      this.onSelectVisibilty.emit(value);
  }

  previousChoice() : void {
    this.onPrevious.emit();
  }

  ngOnInit(): void {
  }

}

export class VisibilityParam {
  public visibilty?:number;
  public communityId?:number;
}
