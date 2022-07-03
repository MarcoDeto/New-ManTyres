import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Response } from '../Models/response.model';

@Injectable({
  providedIn: 'root'
})

export class ConnectionInfoService {

  constructor(
    private http: HttpClient,
  ) { }

  getIPAddress(): Observable<any> {
    return this.http.get("https://api64.ipify.org/?format=json");
  }

  getLocation(ipAddress: string): Observable<any> {
    return this.http.get("https://ipwho.is/" + ipAddress);
  }

  GetLanguages(): Observable<Response> {
    return this.http.get<Response>(environment.languages + 'GetAll');
  }

  GetCurrencies(): Observable<any> {
    return this.http.get<Response>(environment.currencies + 'GetAll');
  }

  getCountryCode() { return localStorage.getItem("country_code"); }
  setCountryCode(country_code: string) {
    localStorage.setItem('country_code', country_code);
  }
}
