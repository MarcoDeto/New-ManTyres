import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Country } from 'src/app/Shared/Models/country.model';
import { Currency } from 'src/app/Shared/Models/currency.model';
import { Language } from 'src/app/Shared/Models/language.model';
import { Response } from 'src/app/Shared/Models/response.model';
import { ConnectionInfoService } from 'src/app/Shared/Services/connectionInfo.service';
import { CountryService } from 'src/app/Shared/Services/country.service';

@Component({
  selector: 'app-local-settings',
  templateUrl: './local-settings.component.html',
  styleUrls: ['./local-settings.component.scss']
})
export class LocalSettingsComponent implements OnInit {

  languages: Language[] = [];
  currencies: Currency[] = [];
  country: Country | undefined;
  country_code: string | undefined;

  constructor(
    public dialogRef: MatDialogRef<LocalSettingsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any | undefined,
    public service: ConnectionInfoService,
    public countryService: CountryService,
  ) { }

  ngOnInit() {
    this.getLanguages();
  }

  closeDialog() {
    this.dialogRef.close(null);
  }

  getLanguages() {
    this.service.GetLanguages().subscribe(
      (res: Response) => {
        let langs = [...res.content];
        this.country_code = this.service.getCountryCode()!;
        this.getCurrencies();
        const firstLanguage: Language = res.content.find((x: any) => x.code.includes(this.country_code))!;
        const firstLanguageIndex: number = res.content.findIndex((x: any) => x.code.includes(this.country_code))!;
        this.languages = [];
        this.languages.push(firstLanguage);
        langs.splice(firstLanguageIndex, 1);
        this.languages = this.languages.concat(langs);
        //this.languages = [...res.content];
      }
    );
  }

  getCurrencies() {
    if (this.country_code) {
      this.countryService.GetByISO(this.country_code).subscribe(
        (res: Response) => {
          this.country = res.content;
          this.service.GetCurrencies().subscribe(
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
        });
    }
  }

  checkLangCode(language: Language): boolean {
    if (this.country_code && language.code.includes(this.country_code)) {
      return true;
    }
    return false;
  }

  checkCurrCode(currency: Currency): boolean {
    if (currency && this.country && currency.code == this.country.currencyCode) {
      return true;
    }
    return false;
  }
}
