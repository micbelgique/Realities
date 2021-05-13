import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportsPannelComponent } from './reports-pannel.component';

describe('ReportsPannelComponent', () => {
  let component: ReportsPannelComponent;
  let fixture: ComponentFixture<ReportsPannelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportsPannelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportsPannelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
