import { Router } from '@angular/router';

import { AlreadySignedInGuard } from './already-signed-in.guard';

class MockRouter {
  navigate(path) {}
}

describe('AlreadySignedInGuard.canActivate()', () => {
  const router: Router = new MockRouter() as Router;
  const alreadySignedInGuard: AlreadySignedInGuard = new AlreadySignedInGuard(router);

  describe('canActivate()', () => {
    it('should have returned true for a non-logged in user', () => {
      expect(alreadySignedInGuard.canActivate()).toEqual(true);
    });

    it('should have returned false for a logged in user', () => {
      localStorage.setItem('currentUser', 'currentUser');
      expect(alreadySignedInGuard.canActivate()).toEqual(false);
      localStorage.removeItem('currentUser');
    });
  });
});
