import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Cliente } from '../../Shared/Models/clienti.model';

@Injectable({
  providedIn: 'root'
})
export class ClientiService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getCliente(id: number): Observable<Response> {
    return this.http.get<Response>(environment.clienti + '/GetById?id=' + id);
  }

  getAllSkipTake(skip: number, take: number, orderByNome: boolean, filter: string): Observable<Response> {
    let orderByNomeLink = orderByNome ? "&orderByTarga=" + orderByNome : "";
    return this.http.get<Response>(environment.clienti + '/GetAllSkipTake?skip=' + skip + '&take=' + take + orderByNomeLink + '&filter=' + filter);
  }

  getAllClienti(): Observable<Response> {
    return this.http.get<Response>(environment.clienti + '/GetAll');
  }

  addCliente(cliente: Cliente): Observable<Response> {
    return this.http.post<Response>(environment.clienti + '/Add', cliente).pipe(catchError((e: any) => {
          if (e.error.content)
            e.error.content.errors.forEach(element => this.toastr.error(element.description, 'Errore.'));
          else
            this.toastr.error(e.error.errorMessage, 'Errore.');
          return throwError(e);
    }));
  }

  editCliente(cliente: Cliente): Observable<Response> {
    return this.http.put<Response>(environment.clienti + '/Update', cliente);
  }

  deleteCliente(id: number): Observable<Response> {
    return this.http.delete<Response>(environment.clienti + '/Deactivate?id=' + id);
  }
}
