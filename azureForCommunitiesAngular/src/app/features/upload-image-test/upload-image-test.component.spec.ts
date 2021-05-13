import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadImageTestComponent } from './upload-image-test.component';

describe('UploadImageTestComponent', () => {
  let component: UploadImageTestComponent;
  let fixture: ComponentFixture<UploadImageTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadImageTestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadImageTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
