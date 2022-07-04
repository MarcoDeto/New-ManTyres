import { MongoDocument } from './base.model';

export interface Period {
   open: Hours;
   close: Hours;
}

export interface Hours {
   day: number;
   time: string | null;
}

export interface Place extends MongoDocument {
   address: string | null;
   locality: string | null;
   lat: number;
   lng: number;
   country: string | null;
   administrative_area_level_1: string | null;
   administrative_area_level_2: string | null;
   administrative_area_level_3: string | null;
   isO2: string | null;
   postal_code: string | null;
   business_status: string | null;
   name: string | null;
   website: string | null;
   open_Now: boolean;
   periods: Period[];
   weekday_Text: string[];
   phone_Number: string | null;
   international_Phone_Number: string | null;
   google_Photos: string[];
   google_Place_Id: string | null;
   google_Rating: number;
   google_Url: string | null;
   email: string | null;
   verified: boolean;
}
