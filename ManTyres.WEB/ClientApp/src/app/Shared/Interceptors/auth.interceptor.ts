import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap  } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from "src/app/Account/Services/user.service";


@Injectable()
export class AuthInterceptor implements HttpInterceptor{

  token = "";

  constructor(private router: Router, private userService: UserService) {
    this.token = this.userService.getToken()!;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req);
    this.token = this.userService.getToken()!;
    if (this.token != "" && this.userService.isLoggedIn()){
      const clonedReq = req.clone({ headers: req.headers.set('Authorization', 'Bearer '+ this.token) });
      return next.handle(clonedReq).pipe(
        tap(
          succ => {
            this.userService.yes();
          },
          error => {
            if(error.status == 401){
              this.userService.removeToken();
              this.router.navigate(['/login']);
            }
            else if(error.status == 403){
              this.router.navigate(['/forbidden']);
            }
          }
        )
      );
    }
    else{
      return next.handle(req.clone());
    }

  }

}
