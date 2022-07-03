import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Country } from '../Models/country.model';
import { Currency } from '../Models/currency.model';
import { Language } from '../Models/language.model';
import { Response } from '../Models/response.model';
import { CountryService } from './country.service';

@Injectable({
  providedIn: 'root'
})

export class ConnectionInfoService {

  languages: Language[] = [];
  languages_suggested: Language[] = [];
  currencies: Currency[] = [];
  country: Country | null = null;
  country_code: string | null = null;
  currency_code: string | null = null;
  langLocalStorage = localStorage.getItem('lang');
  currencyLocalStorage = localStorage.getItem('currency');

  DEFAULT_CURRENCY: string = 'EUR';

  constructor(
    private http: HttpClient,
    public countryService: CountryService,
    private translate: TranslateService
  ) { }

  getIPAddress(): Observable<any> {
    return this.http.get('https://api64.ipify.org/?format=json');
  }

  getLocation(ipAddress: string): Observable<any> {
    return this.http.get('https://ipwho.is/' + ipAddress);
  }

  GetLanguages(): Observable<Response> {
    return this.http.get<Response>(environment.languages + 'GetAll');
  }

  GetCurrencies(): Observable<any> {
    return this.http.get<Response>(environment.currencies + 'GetAll');
  }

  getCountryCode() { return localStorage.getItem('country_code'); }
  setCountryCode(country_code: string) {
    localStorage.setItem('country_code', country_code);
  }

  getBrowserLang(): string {
    const result = this.translate.getBrowserLang();
    if (result && result.match(/en|fr|es|it|de/)) {
      return result;
    }
    return 'en';
  }

  getLangCurrencies(country_code: string) {
    return this.http.get<Response>(
      environment.firstConnection + 
      'GetCurrLangs?country_code=' + 
      country_code
    );
  }
  setLangCurrency() {
    this.country_code = this.getCountryCode();
    if (this.country_code) {
      this.getLangCurrencies(this.country_code).subscribe(
        (res: any) => {
          this.languages = res.languages;
          this.currencies = res.currencies;
          this.country = res.country;
          this.languages_suggested = res.languages_suggested;
        }
      )
    }
  }

  getLanguages() {
    this.GetLanguages().subscribe(
      (res: Response) => {
        let langs = [...res.content];
        this.country_code = this.getCountryCode()!;
        this.getCurrencies();
        const firstLanguage: Language = res.content.find((x: any) => x.code.includes(this.country_code))!;
        const firstLanguageIndex: number = res.content.findIndex((x: any) => x.code.includes(this.country_code))!;
        this.languages = [];
        this.languages.push(firstLanguage);
        langs.splice(firstLanguageIndex, 1);
        this.languages = this.languages.concat(langs);
      }
    );
  }

  getCurrencies() {
    if (this.country_code) {
      this.countryService.GetByISO(this.country_code).subscribe(
        (res: Response) => {
          this.country = res.content;
          this.GetCurrencies().subscribe(
            (res: Response) => {
              let currs = [...res.content];
              const firstCurrency: Currency = res.content.find((x: any) => x.code == this.country?.currencyCode);
              const firstCurrencyIndex: number = res.content.findIndex((x: any) => x.code == this.country?.currencyCode);
              this.currencies = [];
              this.currencies.push(firstCurrency);
              currs.splice(firstCurrencyIndex, 1);
              this.currencies = this.currencies.concat(currs);
            }
          );
        }
      );
    }
  }

  getLangLocalStorage() { return localStorage.getItem('lang'); }
  getCurrencyLocalStorage() { return localStorage.getItem('currency'); }

  initLanguage() {
    this.langLocalStorage = this.getLangLocalStorage();
    if (this.langLocalStorage) {
      this.translate.setDefaultLang(this.langLocalStorage);
      this.translate.use(this.langLocalStorage);
    } else {
      localStorage.setItem('lang', this.getBrowserLang());
      this.translate.setDefaultLang(this.getBrowserLang());
      this.translate.use(this.getBrowserLang());
    }
  }

  initCurrency() {
    this.currencyLocalStorage = this.getCurrencyLocalStorage();
    if (this.currencyLocalStorage) {
      this.currency_code = this.currencyLocalStorage;
    } else {
      localStorage.setItem('currency', this.DEFAULT_CURRENCY);
      this.currency_code = this.DEFAULT_CURRENCY;
    }
  }
}
