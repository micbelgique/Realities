import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAnchorsComponent } from './user-anchors.component';

describe('UserAnchorsComponent', () => {
  let component: UserAnchorsComponent;
  let fixture: ComponentFixture<UserAnchorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAnchorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAnchorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
