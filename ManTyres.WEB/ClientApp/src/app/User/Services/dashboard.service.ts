import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Inventario } from '../../Shared/Models/inventario.model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  Pneumatici(): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/Pneumatici');
  }

  Veicoli(): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/Veicoli');
  }

  Clienti(): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/Clienti');
  }

  Utenti(): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/Utenti');
  }

  ChartGiornalieroPneumatici(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGiornalieroPneumatici?month=' + month + '&ci=' + cultureInfo);
  }

  ChartGlobalePneumatici(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGlobalePneumatici?month=' + month + '&ci=' + cultureInfo);
  }

  ChartGiornalieroVeicoli(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGiornalieroVeicoli?month=' + month + '&ci=' + cultureInfo);
  }

  ChartGlobaleVeicoli(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGlobaleVeicoli?month=' + month + '&ci=' + cultureInfo);
  }

  ChartGiornalieroClienti(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGiornalieroClienti?month=' + month + '&ci=' + cultureInfo);
  }

  ChartGlobaleClienti(month: number, cultureInfo: string): Observable<Response> {
    return this.http.get<Response>(environment.dashboard + '/ChartGlobaleClienti?month=' + month + '&ci=' + cultureInfo);
  }

}
