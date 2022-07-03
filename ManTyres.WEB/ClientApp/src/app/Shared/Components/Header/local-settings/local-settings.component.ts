import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { Country } from 'src/app/Shared/Models/country.model';
import { Currency } from 'src/app/Shared/Models/currency.model';
import { Language } from 'src/app/Shared/Models/language.model';
import { ConnectionInfoService } from 'src/app/Shared/Services/connectionInfo.service';
import { CountryService } from 'src/app/Shared/Services/country.service';

@Component({
  selector: 'app-local-settings',
  templateUrl: './local-settings.component.html',
  styleUrls: ['./local-settings.component.scss']
})
export class LocalSettingsComponent implements OnInit, OnDestroy {

  languages: Language[] = [];
  languages_suggested: Language[] = [];
  currencies: Currency[] = [];
  country: Country | null = null;
  country_code: string | null = null;
  currency_code: string | null = null;
  isLanguagePage: boolean = true;
  indice: number = 0;
  caricamento: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<LocalSettingsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any | undefined,
    public service: ConnectionInfoService,
    public countryService: CountryService,
    private translate: TranslateService
  ) { }

  ngOnDestroy(): void { }

  ngOnInit() {
    this.caricamento = true;
    this.service.initLanguage();
    this.service.initCurrency();
    if (this.service.languages.length != 0) { this.languages = this.service.languages; }
    if (this.service.languages_suggested.length != 0) { this.languages_suggested = this.service.languages_suggested; }
    if (this.service.currencies.length != 0) { this.currencies = this.service.currencies; }
    if (this.service.country) { this.country = this.service.country; }
    this.currency_code = this.service.currency_code;
    this.country_code = this.service.country_code;
    if (this.languages.length != 0 && this.country_code || this.country && this.currencies.length != 0) {
      this.caricamento = false;
    }
  }

  loadLanguages_suggested(): Language[] {
    let result: Language[] = [...this.languages_suggested];
    const langIndex = this.languages_suggested.findIndex(x => x.code.includes(this.country_code!))
    result.splice(langIndex, 1);
    return result;
  }

  closeDialog() {
    this.dialogRef.close(null);
  }

  checkLangCode(language: Language): boolean {
    if (this.country_code && language.code.includes(this.country_code)) {
      return true;
    }
    return false;
  }

  checkCurrCode(currency: Currency): boolean {
    if (currency && this.country && currency.code == this.currency_code) {
      return true;
    }
    return false;
  }

  IsLanguagePage($event: any) {
    if ($event == 0) {
      this.isLanguagePage = true;
    }
    else {
      this.isLanguagePage = false;
    }
  }

  setLanguage(lang: string) {
    if (lang && lang.match(/en|fr|es|it|de/)) {
      localStorage.setItem('lang', lang);
      this.translate.setDefaultLang(lang);
      this.translate.use(lang);
      this.country_code = lang;
      this.service.country_code = lang;
    }
  }

  setCurrency(currency: string) {
    if (currency) {
      localStorage.setItem('currency', currency);
      this.currency_code = currency;
      this.service.currency_code = currency;
    }
  }
}
