import { TestBed } from '@angular/core/testing';

import { SignalRClientService } from './signal-rclient-service';

describe('SignalRClientService', () => {
  let service: SignalRClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalRClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
