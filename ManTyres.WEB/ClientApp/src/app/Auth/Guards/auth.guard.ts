import { Injectable, OnDestroy } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from '../Services/user.service';

@Injectable()
export class AuthGuard implements CanActivate, OnDestroy {
  subscribers: Subscription[] = [];
  constructor(
    private router: Router, 
    private userService: UserService
  ) {}

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return true;
    const token = this.userService.getToken();
    if (token && this.userService.isLoggedIn()) {
      const payload = JSON.parse(window.atob(token!.split('.')[1]));
      const role = payload.role.toLowerCase();
      const roles = route.data['permittedRoles'] as Array<string>;
      if (roles.includes(role)) {
        if (this.userService.roleMatch(roles)) {
          return true;
        }
        else {
          this.router.navigate(['forbidden']);
          return false;
        }
      }
      return true;
    }
    else {
      this.router.navigate(['/forbidden'], { queryParams: { ru: state.url } });
      return false;
    }
  }

}
