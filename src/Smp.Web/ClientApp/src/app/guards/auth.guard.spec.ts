import { Router, RouterStateSnapshot } from '@angular/router';

import { AuthGuard } from './auth.guard';

class MockRouter {
  navigate(path) {}
}

describe('AuthGuard.canActivate()', () => {
  const router: Router = new MockRouter() as Router;
  const authGuard: AuthGuard = new AuthGuard(router);
  let state: RouterStateSnapshot = { url: '/' } as RouterStateSnapshot;

  describe('canActivate()', () => {
    it('should have returned true for a logged in user', () => {
      localStorage.setItem('currentUser', 'currentUser');
      expect(authGuard.canActivate(null, state)).toEqual(true);
      localStorage.removeItem('currentUser');
    });

    it('should have returned false for a non-logged in user', () => {
      expect(authGuard.canActivate(null, state)).toEqual(false);
    });
  });
});
