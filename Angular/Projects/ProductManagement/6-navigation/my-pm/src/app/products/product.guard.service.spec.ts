import { TestBed } from '@angular/core/testing';

import { Product.GuardService } from './product.guard.service';

describe('Product.GuardService', () => {
  let service: Product.GuardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Product.GuardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
