import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Place } from '../Models/place.model';

@Injectable({
  providedIn: 'root'
})

export class PlaceService {

  constructor(
    private http: HttpClient,
  ) { }

  getPlaces() {
    return this.http.get(environment.place + 'GetPlaces');
  }

  GetByPlacesId(places_id: string[]) {
    return this.http.patch(environment.place + 'GetByPlacesId', places_id);
  }

  addPlaces(request: Place[]) {
    return this.http.post(environment.place + 'AddPlaces', request);
  }

  updatePlace(request: Place) {
    return this.http.put(environment.place + 'UpdatePlace', request);
  }
}
