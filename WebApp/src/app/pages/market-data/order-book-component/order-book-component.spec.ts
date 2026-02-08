import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderBookComponent } from './order-book-component';

describe('OrderBookComponent', () => {
  let component: OrderBookComponent;
  let fixture: ComponentFixture<OrderBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderBookComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderBookComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
