import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnchorsMapComponent } from './anchors-map.component';

describe('AnchorsMapComponent', () => {
  let component: AnchorsMapComponent;
  let fixture: ComponentFixture<AnchorsMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnchorsMapComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnchorsMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
