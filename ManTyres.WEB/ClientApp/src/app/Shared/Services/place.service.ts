import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Place } from '../Models/place.model';
import { Response } from '../Models/response.model';

@Injectable({
  providedIn: 'root'
})

export class PlaceService {

  placeSelected: Place | undefined;

  constructor(
    private http: HttpClient,
  ) { }

  getImageUrl(photo_url: string) {
    return environment.GOOGLE_PLACE_PHOTO+photo_url+'&key='+environment.GOOGLE_MAPS_API_KEY;
  }

  getPlaces() {
    return this.http.get(environment.place + 'GetPlaces');
  }

  getNear(LAT: number, LNG: number): Observable<Response> {
    return this.http.get<Response>(environment.place + 'GetNear?LAT='+LAT+'&LNG='+LNG);
  }

  GetByPlacesId(places_id: string[]): Observable<Response> {
    return this.http.patch<Response>(environment.place + 'GetByPlacesId', places_id);
  }

  GetById(places_id: string): Observable<Response> {
    return this.http.get<Response>(environment.place + 'Get?Id=' + places_id);
  }

  addPlace(request: Place) {
    return this.http.post(environment.place + 'AddPlace', request);
  }

  addPlaces(request: Place[]) {
    return this.http.post(environment.place + 'AddPlaces', request);
  }

  updatePlace(request: Place) {
    return this.http.put(environment.place + 'UpdatePlace', request);
  }
}
