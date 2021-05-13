import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnchorTemplateComponent } from './anchor-template.component';

describe('AnchorTemplateComponent', () => {
  let component: AnchorTemplateComponent;
  let fixture: ComponentFixture<AnchorTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnchorTemplateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnchorTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
