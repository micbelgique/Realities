import { EventEmitter, Output } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';
import { ShopCategory } from 'src/app/shared/models/shop/shop-category.model';

@Component({
  selector: 'app-shop-category',
  templateUrl: './shop-category.component.html',
  styleUrls: ['./shop-category.component.css']
})
export class ShopCategoryComponent implements OnInit {

  @Input() category?:ShopCategory;

  @Output() onSelectModel: EventEmitter<string> = new EventEmitter();
  suggestionWasClicked(value: string): void {
      this.onSelectModel.emit(value);
  }

  constructor() { }

  ngOnInit(): void {
  }

  onItemSelected(model:string)
  {
    alert(model);
  }

}
