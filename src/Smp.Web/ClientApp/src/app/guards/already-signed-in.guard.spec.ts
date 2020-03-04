import { AlreadySignedInGuard } from './already-signed-in.guard';
import { Router } from '@angular/router';

class MockRouter {
  navigate(path) {}
}

describe('AlreadySignedInGuard canActivate', () => {
  afterEach(() => {
    localStorage.removeItem('currentUser');
  })

  const router: Router = new MockRouter() as Router;
  const alreadySignedInGuard = new AlreadySignedInGuard(router);

  it('should return true for a non-logged in user', () => {
    expect(alreadySignedInGuard.canActivate()).toEqual(true);
  });

  it ('should return false for a logged in user', () => {
    localStorage.setItem('currentUser', 'currentUser');

    expect(alreadySignedInGuard.canActivate()).toEqual(false);
  });
});