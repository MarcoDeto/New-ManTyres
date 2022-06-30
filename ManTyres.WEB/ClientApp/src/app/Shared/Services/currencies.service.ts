import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class CurrenciesService {

  constructor(
    private http: HttpClient,
  ) { }

  GetValue(): Observable<any> {
    return this.http.post<any>("http://api.exchangeratesapi.io/v1/latest?access_key=4801364e4c3570e773b931543a4b31ca", null);
  }

}
