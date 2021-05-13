import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { SingleData } from 'src/app/shared/models/api-responses/single-data.model';
import { environment } from 'src/environments/environment';

@Component({
	selector: 'app-upload-image-test',
	templateUrl: './upload-image-test.component.html',
	styleUrls: ['./upload-image-test.component.css']
})
export class UploadImageTestComponent implements OnInit {

	@Output() onUpload: EventEmitter<string> = new EventEmitter();

	selectedFile = null;
	url?:string= undefined;
	uploadUrl?:string= undefined;
	sizeLimit:number= 4000000;
	isOverLimit:boolean = false;
	errorMessage?:string = undefined;


	constructor(private http: HttpClient) { }

	ngOnInit(): void {
		
	}

	onUploadDone(url:string)
	{
		this.onUpload.emit(url);
		this.isOverLimit = true;
	}

	onFileSelected(event: any) {
		if(event.target.files)
		{
			if(event.target.files[0].size > this.sizeLimit)
			{
				this.isOverLimit = true;
				this.errorMessage = "your file is larger than 2mb";
				return;
			}

			this.isOverLimit = false;
			this.errorMessage = "";
			this.selectedFile = event.target.files[0];
			console.log(event.target.files[0]);
			let reader = new FileReader(); 
			reader.readAsDataURL(event.target.files[0]);
			reader.onload = (event:any) => {this.url = event.target.result;}
		}
	}

	upload() {
		let file: File = <File>this.selectedFile!;
		const fd = new FormData();
		fd.append('image', file, file.name);
		this.http.post(environment.apiUrl + '/api/ImageAPI/uploadImage', fd,
			{
				reportProgress: true,
				observe: 'events'
			}).subscribe(
				(event) => {
					switch (event.type) {
						case HttpEventType.UploadProgress:
							console.log('Upload Progress: ' + Math.round(event.loaded / event.total! * 100) + '%');
							break;
						case HttpEventType.Response:
							console.log("Done:");
							console.log(event);
							let res:SingleData<string> = event.body!;
							this.uploadUrl = res.data!;
							this.onUploadDone(this.uploadUrl);
							break;
					}
				},
				err => {
					console.log(err);
				}
			)
	}

}
