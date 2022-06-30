import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Loader } from "@googlemaps/js-api-loader";
import { environment } from "src/environments/environment";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { GeolocationCoordinates } from '../../home/home.component';

@Injectable({
	providedIn: "root",
})
export class MapService {
	// map
	mapZoomDefaultValue: number = 7;
	mapCenter: google.maps.LatLngLiteral = {
		lat: 44.58988245722263,
		lng: 10.38770645464642,
	};

	// load google key
	loader = new Loader({
		apiKey: environment.GOOGLE_MAPS_API_KEY,
	});

	constructor(
		public router: Router,
		private http: HttpClient,
	) { }

	// Geocoding
	GetObjectPosition(
		address: string,
		city: string,
		province: string,
		postalCode: string,
		country: string
	): Observable<any> {
		return this.http.get(
			"https://maps.googleapis.com/maps/api/geocode/json?address=" +
			address +
			city +
			postalCode +
			province +
			country +
			"&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}

	placeDetail(place_id: string): Observable<any> {
		return this.http.get(
			environment.GOOGLE_PLACE_URL +
			"details/json?place_id=" +
			place_id +
			"&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}

	textSearch(query: string): Observable<any> {
		return this.http.get(
			environment.GOOGLE_PLACE_URL + 
			"textsearch/json?query=" +
			query +
			"&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}	
	
	proxySearch(query: string) {
		return this.http.get(
			environment.PROXY_URL + 
			environment.GOOGLE_PLACE_URL + 
			"textsearch/json?query=" +
			query +
			"&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}

	proxyDetail(place_id: string) {
		return this.http.get( 
			environment.PROXY_URL + 
			environment.GOOGLE_PLACE_URL +
			"details/json?place_id=" +
			place_id +
			"&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}

	proxyNearbysearch(coords: GeolocationCoordinates) {
		return this.http.get( 
			environment.PROXY_URL + 
			environment.GOOGLE_PLACE_URL +
			"nearbysearch/json?keyword=gomme&location=" +
			coords.latitude + ', ' + coords.longitude +
			"&radius=1500&key=" +
			environment.GOOGLE_MAPS_API_KEY
		);
	}
}
