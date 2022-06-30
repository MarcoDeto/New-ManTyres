import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
   providedIn: 'root'
})

export class GoogleMapsService {

   constructor(
      private http: HttpClient,
   ) { }

   placeDetail(place_id: string): Observable<any> {
      return this.http.get("https://maps.googleapis.com/maps/api/place/details/json?place_id=" + place_id + "&key=" + environment.GOOGLE_MAPS_API_KEY);
   }

   textSearch(query: string): Observable<any> {
      return this.http.get("https://maps.googleapis.com/maps/api/place/textsearch/json?query=" + query + "&key=" + environment.GOOGLE_MAPS_API_KEY);
   }
}
