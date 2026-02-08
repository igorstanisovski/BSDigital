import { TestBed } from '@angular/core/testing';

import { OrderBookApiService } from './order-book-api-service';

describe('OrderBookApiService', () => {
  let service: OrderBookApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrderBookApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
