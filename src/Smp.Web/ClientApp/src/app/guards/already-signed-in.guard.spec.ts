import { TestBed, async, inject } from '@angular/core/testing';

import { AlreadySignedInGuard } from './already-signed-in.guard';

describe('AlreadySignedInGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AlreadySignedInGuard]
    });
  });

  it('should ...', inject([AlreadySignedInGuard], (guard: AlreadySignedInGuard) => {
    expect(guard).toBeTruthy();
  }));
});
