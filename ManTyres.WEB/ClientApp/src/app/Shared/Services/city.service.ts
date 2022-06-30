import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Place } from '../Models/place.model';

@Injectable({
   providedIn: 'root'
})

export class CityService {

   constructor(
      private http: HttpClient,
   ) { }

   importCities(file: Blob): Observable<HttpEvent<void>> {
      const formData = new FormData();
      formData.append('file', file);
      return this.http.post<HttpEvent<void>>(environment.city + "ImportCities", formData, {
         reportProgress: true
      });
   }

   getAllCities(): Observable<any> {
      return this.http.get<Response>(environment.city + "GetAllCities");
   }

   getCitiesByISO(ISO: string): Observable<any> {
      return this.http.get<Response>(environment.city + "GetCitiesByISO?ISO=" + ISO);
   }

}
