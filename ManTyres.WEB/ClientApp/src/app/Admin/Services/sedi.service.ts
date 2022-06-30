import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Sedi } from '../../Shared/Models/sedi.model';

@Injectable({
  providedIn: 'root'
})
export class SediService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getAllSedi(): Observable<Response> {
    return this.http.get<Response>(environment.sedi + '/GetAll');
  }

  getSedeById(id: any): Observable<Response> {
    return this.http.get<Response>(environment.sedi + '/GetById/' + id);
  }

  addSede(bodyReq: Sedi): Observable<Response> {
    return this.http.post<Response>(environment.sedi + '/Add', bodyReq);
  }

  editSede(bodyReq: Sedi): Observable<Response> {
    return this.http.put<Response>(environment.sedi + '/Update', bodyReq);
  }

  deactiveSede(id: number): Observable<Response> {
    return this.http.delete<Response>(environment.sedi + '/Deactive/' + id);;
  }
}
