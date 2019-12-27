import { TestBed } from '@angular/core/testing';

import { GlobalHelper } from './global';

describe('GlobalHelper', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GlobalHelper = TestBed.get(GlobalHelper);
    expect(service).toBeTruthy();
  });
});
