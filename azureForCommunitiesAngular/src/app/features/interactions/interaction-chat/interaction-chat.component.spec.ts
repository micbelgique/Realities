import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InteractionChatComponent } from './interaction-chat.component';

describe('InteractionChatComponent', () => {
  let component: InteractionChatComponent;
  let fixture: ComponentFixture<InteractionChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InteractionChatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InteractionChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
