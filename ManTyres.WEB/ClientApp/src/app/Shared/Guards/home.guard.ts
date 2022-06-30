import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from "@angular/router";
import { FirstConnectionService } from "../Services/FirstConnection.service";
import { Response } from '../../Shared/Models/response.model';
import { UserService } from "../../Auth/Services/user.service";


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
    const email = this.userService.getEmail();
    if (email) {
      this.router.navigate(['/admin/home']);
      return true;
    }
    else{
      this.router.navigate(['/account']);
      return false;
    }
  }
}
