import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportAnchorDialogComponent } from './report-anchor-dialog.component';

describe('ReportAnchorDialogComponent', () => {
  let component: ReportAnchorDialogComponent;
  let fixture: ComponentFixture<ReportAnchorDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportAnchorDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportAnchorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
