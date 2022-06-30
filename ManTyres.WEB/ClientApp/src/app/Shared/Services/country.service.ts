import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Place } from '../Models/place.model';

@Injectable({
   providedIn: 'root'
})

export class CountryService {

   constructor(
      private http: HttpClient,
   ) { }

   getAllCuntries(): Observable<any> {
      return this.http.get<Response>(environment.country + "GetAllCountries");
   }

   importCuntries(file: Blob): Observable<HttpEvent<void>> {
      const formData = new FormData();
      formData.append('file', file);
      return this.http.post<HttpEvent<void>>(environment.country + "ImportCountries", formData, {
         reportProgress: true
      });
   }
}
