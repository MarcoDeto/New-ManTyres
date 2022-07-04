import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { UserService } from '../Services/user.service';

@Injectable({
  providedIn:'root'
})
export class LoginGuard implements CanActivate {

  constructor(private router: Router, private userService: UserService){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean{
    return true;
    const token = this.userService.getToken();
    if (token && this.userService.isLoggedIn()){
      return true;
    }
    else {
      this.router.navigate(['forbidden']);
      return false;
    }
  }
}
