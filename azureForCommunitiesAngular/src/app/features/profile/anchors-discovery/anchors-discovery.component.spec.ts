import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnchorsDiscoveryComponent } from './anchors-discovery.component';

describe('AnchorsDiscoveryComponent', () => {
  let component: AnchorsDiscoveryComponent;
  let fixture: ComponentFixture<AnchorsDiscoveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnchorsDiscoveryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnchorsDiscoveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
