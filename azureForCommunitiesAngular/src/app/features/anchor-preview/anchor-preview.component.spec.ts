import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnchorPreviewComponent } from './anchor-preview.component';

describe('AnchorPreviewComponent', () => {
  let component: AnchorPreviewComponent;
  let fixture: ComponentFixture<AnchorPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnchorPreviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnchorPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
