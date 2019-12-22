import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class AlreadySignedInGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if ((state.url === '/sign-in' || '/sign-up') && localStorage.getItem('currentUser')) {
      this.router.navigate(['']);
      return false;
    }

    return true;
  }
}
