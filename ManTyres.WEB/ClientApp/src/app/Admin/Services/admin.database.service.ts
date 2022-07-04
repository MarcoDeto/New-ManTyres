import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Response } from 'src/app/Shared/Models/response.model';

@Injectable({
  providedIn: 'root'
})
export class AdminDatabaseService {

  constructor(
    private http: HttpClient,
  ) { }
  // popolaDatabase(bodyReq): Observable<Response> {
  //   return this.http.post<Response>(environment.database, bodyReq)
  //   .pipe(
  //     catchError((e:any) => {
  //       if(e.error.content)
  //         e.error.content.errors.forEach(element => this.toastr.error(element.description, 'Errore.'));
  //       else
  //         this.toastr.error(e.error.errorMessage, 'Errore.');
  //       return throwError(e);
  //     })
  //   );
  // }
  tracciatoClienti(): Observable<Blob> {
    return this.http.get(environment.excel + '/TracciatoClienti', { responseType: 'blob' });
  }

  tracciatoVeicoli(): Observable<Blob> {
    return this.http.get(environment.excel + '/TracciatoVeicoli', { responseType: 'blob' });
  }

  tracciatoPneumatici(): Observable<Blob> {
    return this.http.get(environment.excel + '/TracciatoPneumatici', { responseType: 'blob' });
  }

  caricaTutto(file: Blob, sedeId: number, userId: string, cultureInfo: string): Observable<Response> {
    const formData = new FormData();
    formData.append('file', file);
    let sedeIdLink = '&sedeId=' + sedeId;
    let userIdLink = '&userId=' + userId;
    if (sedeId == null) { sedeIdLink = ''; }
    if (userId == null) { userIdLink = ''; }
    return this.http.post<Response>(environment.excel + '/ImportAll?ci=' + cultureInfo + sedeIdLink + userIdLink, formData, {
      reportProgress: true
    });
  }

  caricaClienti(file: Blob): Observable<HttpEvent<void>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<HttpEvent<void>>(environment.excel + '/ImportClienti', formData, {
      reportProgress: true
    });
  }

  caricaVeicoli(file: Blob): Observable<HttpEvent<void>> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<HttpEvent<void>>(environment.excel + '/ImportVeicoli', formData, {
      reportProgress: true
    });
  }
}
