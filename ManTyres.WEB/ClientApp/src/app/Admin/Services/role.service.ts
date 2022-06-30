import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Role } from 'src/app/Shared/Models/role.model';
import { Response } from 'src/app/Shared/Models/response.model';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getAllRoles(): Observable<Response> {
    return this.http.get<Response>(environment.role)
    .pipe(
      catchError((e:any) => {
        this.toastr.error(e.error.errorMessage, 'Errore.');
        return throwError(e);
      })
    );
  }

  getRole(id: string): Observable<Response> {
    return this.http.get<Response>(environment.role + '/' + id)
    .pipe(
      catchError((e:any) => {
        this.toastr.error(e.error.errorMessage, 'Errore.');
        return throwError(e);
      })
    );
  }

  postRole(bodyReq: Role): Observable<Response> {
    return this.http.post<Response>(environment.role, bodyReq)
    .pipe(
      catchError((e:any) => {
        this.toastr.error(e.error.errorMessage, 'Errore.');
        return throwError(e);
      })
    );
  }

  putRole(bodyReq: Role): Observable<Response> {
    return this.http.put<Response>(environment.role, bodyReq)
    .pipe(
      catchError((e:any) => {
        this.toastr.error(e.error.errorMessage, 'Errore.');
        return throwError(e);
      })
    );
  }

  deleteRole(id: string): Observable<Response> {
    return this.http.delete<Response>(environment.role + '/' + id)
    .pipe(
      catchError((e:any) => {
        this.toastr.error(e.error.errorMessage, 'Errore.');
        return throwError(e);
      })
    );
  }
}
