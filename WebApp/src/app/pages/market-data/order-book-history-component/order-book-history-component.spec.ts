import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderBookHistoryComponent } from './order-book-history-component';

describe('OrderBookHistoryComponent', () => {
  let component: OrderBookHistoryComponent;
  let fixture: ComponentFixture<OrderBookHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderBookHistoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderBookHistoryComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
