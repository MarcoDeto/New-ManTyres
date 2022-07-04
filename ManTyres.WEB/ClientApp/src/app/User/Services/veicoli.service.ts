import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { UserFilter, Utenza, User } from 'src/app/Shared/Models/user.model';
import { Response } from 'src/app/Shared/Models/response.model';
import { Veicolo } from '../../Shared/Models/veicoli.mdel';

@Injectable({
  providedIn: 'root'
})
export class VeicoliService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getAllVeicoliForSelectList(): Observable<Response> {
    return this.http.get<Response>(environment.veicoli + '/GetAllForSelectList');
  }

  getAllVeicoli(): Observable<Response> {
    return this.http.get<Response>(environment.veicoli + '/GetAll');
  }

  getAllSkipTake(skip: number, take: number, orderByTarga: boolean, targa: string): Observable<Response> {
    let orderByTargaLink = orderByTarga ? '&orderByTarga=' + orderByTarga : '';
    return this.http.get<Response>(environment.veicoli + '/GetAllSkipTake?skip=' + skip + '&take=' + take + orderByTargaLink + '&targa=' + targa);
  }

  getVeicolo(id: number): Observable<Response> {
    return this.http.get<Response>(environment.veicoli + '/GetById/' + id);
  }

  getByclienteId(clienteId: number): Observable<Response> {
    return this.http.get<Response>(environment.veicoli + '/GetByClienteId/' + clienteId);
  }

  addVeicolo(veicolo: Veicolo): Observable<Response> {
    return this.http.post<Response>(environment.veicoli + '/Add', veicolo);
  }

  editVeicolo(veicolo: Veicolo): Observable<Response> {
    return this.http.put<Response>(environment.veicoli + '/Update', veicolo);
  }

  deactivateVeicolo(id: number): Observable<Response> {
    return this.http.delete<Response>(environment.veicoli + '/Deactivate/' + id);
  }

  reactivateVeicolo(id: number): Observable<Response> {
    return this.http.delete<Response>(environment.veicoli + '/Reactivate/' + id);
  }
}
