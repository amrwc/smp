import { AuthGuard } from './auth.guard';
import { Router, RouterStateSnapshot } from '@angular/router';

class MockRouter {
  navigate(path) {}
}

describe('AuthGuard canActivate', () => {
  afterEach(() => {
    localStorage.removeItem('currentUser');
  })

  const router: Router = new MockRouter() as Router;
  const authGuard = new AuthGuard(router);
  let state = { url: '/' } as RouterStateSnapshot;

  it('should return true for a logged in user', () => {
    localStorage.setItem('currentUser', 'currentUser');

    expect(authGuard.canActivate(null, state)).toEqual(true);
  });

  it ('should return false for a non-logged in user', () => {
    expect(authGuard.canActivate(null, state)).toEqual(false);
  });
});