import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UploadImageTestComponent } from '../../upload-image-test/upload-image-test.component';

@Component({
  selector: 'app-shop-data-completion',
  templateUrl: './shop-data-completion.component.html',
  styleUrls: ['./shop-data-completion.component.css']
})
export class ShopDataCompletionComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  @Input() completionType:DataCompletionType = DataCompletionType.pannel;

  @Output() onDataCompleted: EventEmitter<DataCompletionParam> = new EventEmitter();
  @Output() onPrevious: EventEmitter<void> = new EventEmitter();

  imageUrl:string = "";
  title:string ="";
  content:string="";

  public isFormComplete:boolean = false;
  
  nextStep(): void {
    let value:DataCompletionParam = new DataCompletionParam();
    value.title =this.title;
    value.text = this.content;
    value.imageUrl = this.imageUrl;
    this.onDataCompleted.emit(value);
  }

  ngOnInit(): void {
  }

  previousChoice() : void {
    this.onPrevious.emit();
  }

  onDataComplete()
  {    
    this.isFormComplete = this.completionType == DataCompletionType.pannel ? (this.title != "" && this.content != "") : (this.imageUrl != "" );
  }

  onImageUpload()
  {
    this.dialog.open(UploadImageTestComponent);
  }

  onImageUploaded(url:string)
  {
    this.imageUrl = url;
    this.onDataComplete();
  }

}

export class DataCompletionParam{
  public title?:string;
  public text?:string;
  public imageUrl?:string;
}

export enum DataCompletionType {
  pannel,
  image
}
