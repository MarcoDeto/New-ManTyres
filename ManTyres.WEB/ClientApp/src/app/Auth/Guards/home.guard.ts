import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { UserService } from '../Services/user.service';
import { Response } from 'src/app/Shared/Models/response.model';
import * as moment from 'moment';
import { FirstConnectionService } from 'src/app/Shared/Services/FirstConnection.service';


@Injectable({
  providedIn:'root'
})
export class HomeGuard implements CanActivate {

  constructor(
    private router: Router,
    private userService: UserService,
    private setupService: FirstConnectionService
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean{
    this.router.navigate(['/admin/home']);
    return true;
    const token = this.userService.getToken();
    if (token && this.userService.isLoggedIn()) {
      var payload = JSON.parse(window.atob(token!.split('.')[1]));
      switch(payload.role.toLowerCase()){
        case 'admin':
          this.router.navigate(['/admin/home']);
          return true;
        case 'user':
          this.router.navigate(['/inventario']);
          return true;
        default:
          this.router.navigate(['forbidden']);
          return false;
      }
    }
    else{
      this.router.navigate(['/account']);
      return false;
    }
  }
}
