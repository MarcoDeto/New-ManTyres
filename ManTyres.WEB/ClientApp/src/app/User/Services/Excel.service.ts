import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { FirstConnectionService } from '../../Shared/Services/FirstConnection.service';

@Injectable({
  providedIn: 'root'
})
export class ExcelService {

  constructor(
    private http: HttpClient,
    private service: FirstConnectionService
  ) { }

  exportClienti(): Observable<Blob> {
    return this.http.post(environment.excel + "/ExportClienti", this.service.clienti, { responseType: 'blob' });
  }

  exportVeicoli(): Observable<Blob> {
    return this.http.post(environment.excel + "/ExportVeicoli", this.service.veicoli, { responseType: 'blob' });
  }

  exportPneumatici(sede: number, stagione: number, orderByDesc: boolean): Observable<Blob> {
    return this.http.post(environment.excel + "/ExportPneumatici", this.service.inventario, { responseType: 'blob' });
  }

  exportInventario(sede: number, stagione: number, orderByDesc: boolean): Observable<Blob> {
    return this.http.post(environment.excel + "/ExportInventario", this.service.inventario, { responseType: 'blob' });
  }

  exportArchivio(sede: number, stagione: number, inizioOrderByDesc: boolean | null, fineOrderByDesc: boolean | null): Observable<Blob> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.post(environment.excel + "/ExportArchivio", this.service.inventario, { responseType: 'blob' });
  }

  exportCestino(sede: number, stagione: number, inizioOrderByDesc: boolean, fineOrderByDesc: boolean): Observable<Blob> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.post(environment.excel + "/ExportCestino", this.service.inventario, { responseType: 'blob' });
  }
}
