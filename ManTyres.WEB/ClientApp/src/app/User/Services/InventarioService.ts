import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';
import { Response } from 'src/app/Shared/Models/response.model';
import { Depositi } from '../../Shared/Models/depositi.model';
import { Inventario } from '../../Shared/Models/inventario.model';

@Injectable({
  providedIn: 'root'
})
export class InventarioService {

  constructor(
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  getInventario(skip: number, take: number, sede: number, stagione: number, orderByDesc: boolean): Observable<Response> {
    return this.http.get<Response>(environment.inventario + "/GetInventario?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + "&orderByDesc=" + orderByDesc);
  }

  getInventarioByTarga(skip: number, take: number, sede: number, stagione: number, orderByDesc: boolean, targa: string): Observable<Response> {
    let Targa = "&targa=" + targa;
    if (targa == null) { Targa = ""; }
    return this.http.get<Response>(environment.inventario + "/GetInventarioByTarga?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + "&orderByDesc=" + orderByDesc + Targa);
  }

  getArchivio(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean, fineOrderByDesc: boolean): Observable<Response> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.get<Response>(environment.inventario + "/GetArchivio?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + inizioOrderByDescLink + fineOrderByDescLink);
  }

  GetArchivioByTarga(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean, fineOrderByDesc: boolean, targa: string): Observable<Response> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    let Targa = "&targa=" + targa;
    if (targa == null) { Targa = ""; }
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.get<Response>(environment.inventario + "/GetArchivioByTarga?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + inizioOrderByDescLink + fineOrderByDescLink + Targa);
  }

  DelFromArchivio(entity: Inventario) {
    return this.http.put<Response>(environment.inventario + '/DelFromArchivio', entity);
  }

  getCestino(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean, fineOrderByDesc: boolean): Observable<Response> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.get<Response>(environment.inventario + "/GetCestino?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + inizioOrderByDescLink + fineOrderByDescLink);
  }

  getCestinoByTarga(skip: number, take: number, sede: number, stagione: number, inizioOrderByDesc: boolean, fineOrderByDesc: boolean, targa: string): Observable<Response> {
    let inizioOrderByDescLink = "&inizioOrderByDesc=" + inizioOrderByDesc;
    let fineOrderByDescLink = "&fineOrderByDesc=" + fineOrderByDesc;
    let Targa = "&targa=" + targa;
    if (targa == null) { Targa = ""; }
    if (inizioOrderByDesc == null) { inizioOrderByDescLink = ""; }
    if (fineOrderByDesc == null) { fineOrderByDescLink = ""; }
    return this.http.get<Response>(environment.inventario + "/getCestinoByTarga?skip=" + skip + "&take=" + take + "&sede=" + sede + "&stagione=" + stagione + inizioOrderByDescLink + fineOrderByDescLink + Targa);
  }

  DelFromCestino(entity: Inventario) {
    return this.http.put<Response>(environment.inventario + '/DelFromCestino', entity);
  }

  RipristinaFromCestino(entity: Inventario) {
    return this.http.put<Response>(environment.inventario + '/RipristinaFromCestino', entity);
  }

  RipristinaCestino() {
    return this.http.put<Response>(environment.inventario + '/RipristinaCestino', null);
  }

  SvuotaCestino() {
    return this.http.delete<Response>(environment.inventario + '/SvuotaCestino');
  }
}
