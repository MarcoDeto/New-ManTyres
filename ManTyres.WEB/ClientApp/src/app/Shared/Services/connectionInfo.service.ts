import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

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
    return this.http.get("https://ipapi.co/" + ipAddress + "/json/");
  }
}
