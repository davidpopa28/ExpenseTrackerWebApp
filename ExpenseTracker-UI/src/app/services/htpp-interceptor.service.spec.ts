import { TestBed } from '@angular/core/testing';

import { HtppInterceptorService } from './htpp-interceptor.service';

describe('HtppInterceptorService', () => {
  let service: HtppInterceptorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HtppInterceptorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
