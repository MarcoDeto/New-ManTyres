import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { LoaderService } from '../Services/loader.service';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
    private count = 0;

    constructor(
        private loaderService: LoaderService
    ) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.count++;
        this.loaderService.setHttpProgressStatus(true);
        return next.handle(request).pipe(
            finalize(() => {
                this.count--;
                if (this.count === 0) {
                    this.loaderService.setHttpProgressStatus(false);
                }
            })
        );
    }
}
