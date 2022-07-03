import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Response } from '../Models/response.model';

@Injectable({
   providedIn: 'root'
})

export class CountryService {

   constructor(
      private http: HttpClient,
   ) { }

   getAllCuntries(): Observable<Response> {
      return this.http.get<Response>(environment.country + "GetAll");
   }

   GetByISO(ISO: string): Observable<Response> {
      return this.http.get<Response>(environment.country + "GetByISO?ISO=" + ISO);
   }

   importCuntries(file: Blob): Observable<HttpEvent<void>> {
      const formData = new FormData();
      formData.append('file', file);
      return this.http.post<HttpEvent<void>>(environment.country + "ImportCountries", formData, {
         reportProgress: true
      });
   }
}
