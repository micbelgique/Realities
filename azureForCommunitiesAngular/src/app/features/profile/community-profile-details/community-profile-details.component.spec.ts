import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunityProfileDetailsComponent } from './community-profile-details.component';

describe('CommunityProfileDetailsComponent', () => {
  let component: CommunityProfileDetailsComponent;
  let fixture: ComponentFixture<CommunityProfileDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommunityProfileDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunityProfileDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
