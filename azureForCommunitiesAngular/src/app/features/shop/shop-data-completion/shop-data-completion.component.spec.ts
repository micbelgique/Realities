import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopDataCompletionComponent } from './shop-data-completion.component';

describe('ShopDataCompletionComponent', () => {
  let component: ShopDataCompletionComponent;
  let fixture: ComponentFixture<ShopDataCompletionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShopDataCompletionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShopDataCompletionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
