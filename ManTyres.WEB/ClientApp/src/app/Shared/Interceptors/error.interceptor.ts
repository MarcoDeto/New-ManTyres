import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, throwError } from 'rxjs';
import { catchError, filter, tap } from 'rxjs/operators';
import { Response } from '../Models/response.model';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(
    private toastr: ToastrService,
    private router: Router,
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    /*return next.handle(req).pipe(
      // There may be other events besides the response.
      filter(event => event instanceof HttpResponse),
      tap((event: HttpResponse<any>) => {
        var response = event.body;
        console.log(response);
        if (response && response.code >= 300)
          this.toastr.error(response.errorMessage, 'Errore!');
      }),
      catchError((error: HttpErrorResponse) => {
        console.log(error);
        if (error.status == 401 || error.status == 403)
          this.router.navigate(['forbidden']);
        if (error.error && error.error.errorMessage)
          this.toastr.error(error.error.errorMessage, 'Errore!');
        else
          this.toastr.error(error.message, 'Errore!');
        return throwError(error);
      })
    );*/
    return null;
  }
}
