import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { UserFilter, Utenza, User, UserPassword } from 'src/app/Shared/Models/user.model';
import { Response } from 'src/app/Shared/Models/response.model';

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getUsers(skip: number, take: number, role: string, filtro?: UserFilter): Observable<Response> {
    skip *= take;
    var params = new HttpParams()
      .set('skip', skip.toString())
      .set('take', take.toString())
      .set('role', role.toString());
    if (filtro == null && filtro == undefined)
      filtro = new UserFilter('', '', '', '');

    return this.http.post<Response>(environment.utente + '/get', filtro, { params: params })
      .pipe(
        catchError((e: any) => {
          this.toastr.error(e.error.errorMessage, 'Errore.');
          return throwError(e);
        })
      );
  }

  getAllUsers(): Observable<Response> {
    return this.http.get<Response>(environment.utente + '/GetAll');
  }

  getUser(id: string): Observable<Response> {
    return this.http.get<Response>(environment.utente + '/GetById/' + id);
  }

  GetRolesUser(userid: string): Observable<Response> {
    return this.http.get<Response>(environment.utente + "/GetRolesUser?userId=" + userid);
  }

  postUser(bodyReq: Utenza): Observable<Response> {
    return this.http.post<Response>(environment.utente + "/Create", bodyReq);
  }

  editPassword(bodyReq: UserPassword): Observable<Response> {
    return this.http.put<Response>(environment.password + '/ResetPassword', bodyReq);
  }

  putUser(bodyReq: User): Observable<Response> {
    return this.http.put<Response>(environment.utente + "/Update", bodyReq);
  }

  deleteUser(id: string): Observable<Response> {
    return this.http.delete<Response>(environment.utente + '/Delete/' + id);
  }
}
