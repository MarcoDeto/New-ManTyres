import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TranslateService } from "@ngx-translate/core";
import { ToastrService } from 'ngx-toastr';

export enum RedirectTo {
  Workshop,
  Equipment
}
@Injectable({
  providedIn: 'root'
})
export class PopUpNotificationService {

  constructor(
    private router: Router,
    private translate: TranslateService,
    private toastr: ToastrService,
  ) { }

  show(message: any) {
    if (message) {
      this.toastr.success(this.translate.instant(message));
    } else {
      this.toastr.success('OK');
    }
  }

  error(err: any) {
    if (err) {
      if (err.error) {
        this.toastr.error(this.translate.instant(err.error.message));
      } else {
        this.toastr.error(this.translate.instant(err.message));
      }
    } else {
      this.toastr.error('ERROR');
    }
  }

  /*showAction(message: string, idDBRecord: number, redirectTo: RedirectTo) {
    this.snackbar.openFromComponent(SnackbarComponent, {
      data: {
        message: message,
        actions: true,
      },
      horizontalPosition: "center",
      verticalPosition: "top",
    })
      .afterDismissed().subscribe((data: any) => {
        if (data.dismissedByAction && redirectTo >= 0) { this._goTo(idDBRecord, redirectTo); }
      });
  }

  showActionDistributor() {
    this.snackbar.openFromComponent(SnackbarComponent, {
      data: {
        message: `${this.translate.instant(`RedirectWS`)}`,
        actions: true,
      },
      horizontalPosition: "center",
      verticalPosition: "top",
    })
      .afterDismissed().subscribe((data: any) => {
        if (data.dismissedByAction) { this.router.navigate(['content/distributors']); }
      });
  }*/


  private _goTo(idDBRecord: number, redirectTo: RedirectTo): void {
    this.router.navigate([`content/${redirectTo === (RedirectTo.Workshop) as number ? "workshops" : "equipments"}`, idDBRecord]);
  }
}
