import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { City } from '../Shared/Models/city.mode';
import { PlaceService } from '../Shared/Services/place.service';
import { ConnectionInfoService } from '../Shared/Services/connectionInfo.service';
import { MapService } from '../Shared/Services/map.service';
import { Period, Place } from '../Shared/Models/place.model';
import { environment } from 'src/environments/environment';
import { CityService } from '../Shared/Services/city.service';
import { Response } from '../Shared/Models/response.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent {
  title = 'Home';
  ipAddress: string = '';
  coords: GeolocationCoordinates = {
    latitude: 0,
    longitude: 0
  };
  location: IpLocation | undefined;

  complete: boolean = false;
  GOOGLE_MAPS_API_KEY = environment.GOOGLE_MAPS_API_KEY;
  photo_reference: string = '';

  cities: City[] = [];
  places: Place[] = [];
  placesToAdd: Place[] = [];
  placesNearby: any = [];
  place: Place = {
    address: null,
    locality: null,
    lat: 0,
    lng: 0,
    country: null,
    administrative_area_level_1: null,
    administrative_area_level_2: null,
    administrative_area_level_3: null,
    isO2: null,
    postal_code: null,
    business_status: null,
    name: null,
    website: null,
    open_Now: false,
    periods: [],
    weekday_Text: [],
    phone_Number: null,
    international_Phone_Number: null,
    google_Photos: [],
    google_Place_Id: null,
    google_Rating: 0,
    google_Url: null,
    id: null,
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false,
    email: null,
    verified: false
  };
  google_Places_Id: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private connInfoService: ConnectionInfoService,
    private placesService: PlaceService,
    private mapService: MapService,
    private cityService: CityService,
  ) { }

  ngOnInit() {
    this.getIPAddress();

    /*const query = "gomme Milano";
    this.getPlaces(query);

    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position: any) => {
          this.coords = { latitude: position.coords.latitude, longitude: position.coords.longitude }
        }
      );
    }*/
  }

  getAllPlaces() {
    this.placesService.getPlaces().subscribe(
      (res: any) => {
        if (res != null && res.content != null) {
          this.places = res.content;
        }
      }
    )
  }

  updatePlaces() {
    if (!this.places || this.places.length == 0) { return; }
    this.places.forEach(
      (place: Place) => {
        this.mapService.proxyDetail(place.google_Place_Id!).subscribe(
          (res: any) => {
            this.updatePlace(place, res.result);
          }
        );
      });
  }

  addPlaces() {
    this.placesService.addPlaces(this.places).subscribe(
      res => console.log(res)
    );
  }

  calcTime(start: Date) {
    var end = new Date();
    console.log(end.getTime() - start.getTime());
  }

  getIPAddress() {
    this.connInfoService.getIPAddress().subscribe(
      (res: any) => {
        this.ipAddress = res.ip;
        this.getLocation();
      }
    );
  }

  getLocation() {
    this.connInfoService.getLocation(this.ipAddress).subscribe(
      (res: any) => {
        this.connInfoService.setCountryCode(res.country_code);
        this.location = res;
        this.coords.latitude = this.location!.latitude;
        this.coords.longitude = this.location!.longitude;

        this.getNearPlaces();
        this.proxyNearbysearch();
        //this.getCitiesByISO(this.location!.country);
      }
    );
  }

  getNearPlaces() {
    this.placesService.getNear(this.coords.latitude, this.coords.longitude).subscribe(
      (res: Response) => {
        this.places = res.content;
      }
    );
  }

  proxyNearbysearch() {
    this.mapService.proxyNearbysearch(this.coords).subscribe(
      (res: any) => {
        if (res.results && res.results.lenght > 0) {
          this.placesNearby = res.results;
          console.log(this.placesNearby);
          res.results.forEach((place: any) => {
            this.google_Places_Id.push(place.place_id);
          });
          console.log(this.google_Places_Id);
          this.placesService.GetByPlacesId(this.google_Places_Id).subscribe(
            (res: any) => {
              if (res && res.content) {
                this.places = res.content;
                if (res.content.length != this.placesNearby.length) {
                  console.log("DIVERSO" + (this.placesNearby.length - res.content.length));
                  this.getPlaceNotSaved();
                }
              }
            }
          );
        }
      }
    );
  }

  getPlaceNotSaved() {
    this.places.forEach(place => {
      var index = this.placesNearby.findIndex((x: any) => x.place_id == place.google_Place_Id);
      this.placesNearby.splice(index, 1);
    });
    console.log(this.placesNearby);
    this.placesNearby.forEach((place: any) => {
      this.mapService.proxyDetail(place.place_id!).subscribe(
        (res: any) => {
          this.addPlaceComplete(res.result);
        }
      );
    });
  }

  getCitiesByISO(ISO: string) {
    this.cityService.getCitiesByISO(ISO).subscribe(
      res => {
        this.cities = res.content;
        this.searchMyCustomer(res.content);
      }
    );
  }

  searchMyCustomer(cities: City[]) {
    cities.forEach(city => {
      this.getPlaces("gomme " + city.name + " IT");
    });
    this.complete = true;
  }

  getPlaces(query: string) {
    this.mapService.proxySearch(query).subscribe(
      (firstSearch: any) => {
        firstSearch.results.forEach((element: any) => {
          this.addPlace(element);

          /*this.mapService.proxyDetail(element.place_id).subscribe(
            (res: any) => {
              this.addPlaceComplete(res.result);
            } 
          )*/
        });
      }
    );
  }


  addPlace(data: any) {

    if (this.placesToAdd.findIndex(x => x.address === data.formatted_address) !== -1) {
      return;
    }
    if (data.photos) {
      this.photo_reference = data.photos[0]?.photo_reference;
    }

    const placeToAdd: Place = {
      address: data.formatted_address,
      locality: null,
      lat: data.geometry?.location?.lat,
      lng: data.geometry?.location?.lng,
      country: null,
      administrative_area_level_1: null,
      administrative_area_level_2: null,
      administrative_area_level_3: null,
      isO2: null,
      postal_code: null,
      business_status: data.business_status,
      name: data.name,
      website: data.website,
      open_Now: false,
      periods: [],
      weekday_Text: [],
      phone_Number: null,
      international_Phone_Number: null,
      google_Photos: this.photosInit(data.photos),
      google_Place_Id: data.place_id,
      google_Rating: data.rating,
      google_Url: null,
      id: null,
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false,
      email: null,
      verified: false
    }
    this.placesToAdd.push(placeToAdd);
  }

  addPlaceComplete(data: any) {

    if (this.placesToAdd.findIndex(x => x.address === data.formatted_address) !== -1) {
      return;
    }
    if (data.photos) {
      this.photo_reference = data.photos[0]?.photo_reference;
    }

    const placeToAdd: Place = {
      address: data.formatted_address,
      locality: data.address_components[2]?.long_name,
      lat: data.geometry?.location?.lat,
      lng: data.geometry?.location?.lng,
      country: data.address_components[6]?.long_name,
      administrative_area_level_1: data.address_components[5]?.long_name,
      administrative_area_level_2: data.address_components[4]?.short_name,
      administrative_area_level_3: data.address_components[4]?.long_name,
      isO2: data.address_components[6]?.short_name,
      postal_code: data.address_components[7]?.long_name,
      business_status: data.business_status,
      name: data.name,
      website: data.website,
      open_Now: false,
      periods: this.periodsInit(data.opening_hours?.periods),
      weekday_Text: data.opening_hours?.weekday_text,
      phone_Number: data.formatted_phone_number,
      international_Phone_Number: data.international_phone_number,
      google_Photos: this.photosInit(data.photos),
      google_Place_Id: data.place_id,
      google_Rating: data.rating,
      google_Url: data.url,
      id: null,
      createdAt: new Date(),
      updatedAt: new Date(),
      isDeleted: false,
      email: null,
      verified: false
    }
    this.placesService.addPlace(placeToAdd).subscribe(
      (res: any) => {
        this.placesService.GetByPlacesId(this.google_Places_Id).subscribe(
          (res: any) => {
            this.places = res.content;
          });
      }
    );
  }

  updatePlace(place: Place, data: any) {

    if (data.photos) {
      this.photo_reference = data.photos[0]?.photo_reference;
    }

    place.address = data.formatted_address;
    place.locality = data.address_components[2]?.long_name;
    place.lat = data.geometry?.location?.lat;
    place.lng = data.geometry?.location?.lng;
    place.country = data.address_components[6]?.long_name;
    place.administrative_area_level_1 = data.address_components[5]?.long_name;
    place.administrative_area_level_2 = data.address_components[4]?.short_name;
    place.administrative_area_level_3 = data.address_components[4]?.long_name;
    place.isO2 = data.address_components[6]?.short_name;
    place.postal_code = data.address_components[7]?.long_name;
    place.business_status = data.business_status;
    place.name = data.name;
    place.website = data.website;
    place.open_Now = false;
    place.periods = this.periodsInit(data.opening_hours?.periods);
    place.weekday_Text = data.opening_hours?.weekday_text;
    place.phone_Number = data.formatted_phone_number;
    place.international_Phone_Number = data.international_phone_number;
    place.google_Photos = this.photosInit(data.photos);
    place.google_Place_Id = data.place_id;
    place.google_Rating = data.rating;
    place.google_Url = data.url;
    place.updatedAt = new Date();

    this.placesService.updatePlace(place).subscribe();
  }

  photosInit(photos: any): string[] {
    let result: string[] = [];
    if (!photos || photos.lenght == 0) { return []; }
    photos.forEach(
      (photo: any) => {
        result.push(photo.photo_reference);
      });
    return result;
  }

  periodsInit(periods: any): Period[] {
    let result: Period[] = [];
    if (!periods || periods.lenght == 0) { return []; }
    periods.forEach(
      (period: any) => {
        result.push(this.periodInit(period));
      });
    return result;
  }

  periodInit(period: any): Period {
    return {
      open: {
        day: period?.open?.day, time: period?.open?.time
      },
      close: {
        day: period?.close?.day, time: period?.close?.time
      },
    }
  }
}

export interface GeolocationCoordinates {
  latitude: number;
  longitude: number;
}

export interface IpLocation {
  borders: string;
  calling_code: string;
  capital: string;
  city: string;
  connection: any;
  continent: string;
  continent_code: string;
  country: string;
  country_code: string;
  flag: any;
  ip: string;
  is_eu: boolean;
  latitude: number;
  longitude: number;
  postal: string;
  region: string;
  region_code: string;
  success: true
  timezone: any;
  type: string;

  // asn: string;
  // city: string;
  // continent_code: string;
  // country: string;
  // country_area: number;
  // country_calling_code: string;
  // country_capital: string;
  // country_code: string;
  // country_code_iso3: string;
  // country_name: string;
  // country_population: number;
  // country_tld: string;
  // currency: string;
  // currency_name: string;
  // in_eu: boolean;
  // ip: string;
  // languages: string;
  // latitude: number;
  // longitude: number;
  // org: string;
  // postal: string;
  // region: string;
  // region_code: string;
  // timezone: string;
  // utc_offset: string;
  // version: string;
}