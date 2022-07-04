import { ErrorHandler, Injectable, NgZone } from '@angular/core';
import { ErrorService } from './error.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private errorService: ErrorService, private zone: NgZone) {}

  handleError(error: Error) {

    this.zone.run(() =>
      this.errorService.PostError(
        error.message || 'Undefined client error'
    ));

    console.error('Error from global error handler', error);
  }
}