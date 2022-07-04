import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Depositi } from '../../Shared/Models/depositi.model';

@Injectable({
  providedIn: 'root'
})
export class DepositiService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getAllDepositi(): Observable<Response> {
    return this.http.get<Response>(environment.depositi).pipe(catchError((e: any) => {
      this.toastr.error(e.error.errorMessage, 'Errore.');
      return throwError(e);
    }));
  }

  addDepositi(depositi: Depositi): Observable<Response> {
    return this.http.post<Response>(environment.depositi, depositi).pipe(catchError((e: any) => {
      if (e.error.content)
        e.error.content.errors.forEach((element: any) => this.toastr.error(element.description, 'Errore.'));
      else
        this.toastr.error(e.error.errorMessage, 'Errore.');
      return throwError(e);
    }));
  }
}
