import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { UserFilter, Utenza, User } from '../../Shared/Models/user.model';
import { Response } from '../../Shared/Models/response.model';
import { Role } from '../Models/role.model';
import { Stagioni } from '../Models/stagioni.model';
import { Sedi } from '../Models/sedi.model';

@Injectable({
  providedIn: 'root'
})

export class FirstConnectionService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  GenerateData(): Observable<Response> {
    return this.http.get<Response>(environment.firstConnection + '/GenerateData?password=PasswordSegretissima12345!');
  }

}
