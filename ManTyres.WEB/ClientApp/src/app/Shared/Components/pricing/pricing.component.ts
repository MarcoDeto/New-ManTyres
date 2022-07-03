import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Currencies } from '../../Models/currency.model';
import { ConnectionInfoService } from '../../Services/connectionInfo.service';

@Component({
  selector: 'app-pricing',
  templateUrl: './pricing.component.html',
  styleUrls: ['./pricing.component.scss']
})
export class PricingComponent implements OnInit {
  get currencies() { return Currencies; }
  rates: Rates = {
    AED: 0,
    AFN: 0,
    ALL: 0,
    AMD: 0,
    ANG: 0,
    AOA: 0,
    ARS: 0,
    AUD: 0,
    AWG: 0,
    AZN: 0,
    BAM: 0,
    BBD: 0,
    BDT: 0,
    BGN: 0,
    BHD: 0,
    BIF: 0,
    BMD: 0,
    BND: 0,
    BOB: 0,
    BRL: 0,
    BSD: 0,
    BTC: 0,
    BTN: 0,
    BWP: 0,
    BYN: 0,
    BYR: 0,
    BZD: 0,
    CAD: 0,
    CDF: 0,
    CHF: 0,
    CLF: 0,
    CLP: 0,
    CNY: 0,
    COP: 0,
    CRC: 0,
    CUC: 0,
    CUP: 0,
    CVE: 0,
    CZK: 0,
    DJF: 0,
    DKK: 0,
    DOP: 0,
    DZD: 0,
    EGP: 0,
    ERN: 0,
    ETB: 0,
    EUR: 0,
    FJD: 0,
    FKP: 0,
    GBP: 0,
    GEL: 0,
    GGP: 0,
    GHS: 0,
    GIP: 0,
    GMD: 0,
    GNF: 0,
    GTQ: 0,
    GYD: 0,
    HKD: 0,
    HNL: 0,
    HRK: 0,
    HTG: 0,
    HUF: 0,
    IDR: 0,
    ILS: 0,
    IMP: 0,
    INR: 0,
    IQD: 0,
    IRR: 0,
    ISK: 0,
    JEP: 0,
    JMD: 0,
    JOD: 0,
    JPY: 0,
    KES: 0,
    KGS: 0,
    KHR: 0,
    KMF: 0,
    KPW: 0,
    KRW: 0,
    KWD: 0,
    KYD: 0,
    KZT: 0,
    LAK: 0,
    LBP: 0,
    LKR: 0,
    LRD: 0,
    LSL: 0,
    LTL: 0,
    LVL: 0,
    LYD: 0,
    MAD: 0,
    MDL: 0,
    MGA: 0,
    MKD: 0,
    MMK: 0,
    MNT: 0,
    MOP: 0,
    MRO: 0,
    MUR: 0,
    MVR: 0,
    MWK: 0,
    MXN: 0,
    MYR: 0,
    MZN: 0,
    NAD: 0,
    NGN: 0,
    NIO: 0,
    NOK: 0,
    NPR: 0,
    NZD: 0,
    OMR: 0,
    PAB: 0,
    PEN: 0,
    PGK: 0,
    PHP: 0,
    PKR: 0,
    PLN: 0,
    PYG: 0,
    QAR: 0,
    RON: 0,
    RSD: 0,
    RUB: 0,
    RWF: 0,
    SAR: 0,
    SBD: 0,
    SCR: 0,
    SDG: 0,
    SEK: 0,
    SGD: 0,
    SHP: 0,
    SLL: 0,
    SOS: 0,
    SRD: 0,
    STD: 0,
    SVC: 0,
    SYP: 0,
    SZL: 0,
    THB: 0,
    TJS: 0,
    TMT: 0,
    TND: 0,
    TOP: 0,
    TRY: 0,
    TTD: 0,
    TWD: 0,
    TZS: 0,
    UAH: 0,
    UGX: 0,
    USD: 0,
    UYU: 0,
    UZS: 0,
    VEF: 0,
    VND: 0,
    VUV: 0,
    WST: 0,
    XAF: 0,
    XAG: 0,
    XAU: 0,
    XCD: 0,
    XDR: 0,
    XOF: 0,
    XPF: 0,
    YER: 0,
    ZAR: 0,
    ZMK: 0,
    ZMW: 0,
    ZWL: 0,
  }; 
  currencySelect: Currencies = Currencies.EUR;
  lite: number = 19;
  premium: number = 49;

  constructor(
    private titleService: Title,
    private currenciesService: ConnectionInfoService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.currenciesService.GetCurrencies().subscribe(res => {
      this.rates = res.rates;
    });
  }

  signup() {
    this.router.navigate(['signup']);
  }

  getLiteValue(): number {
    switch (this.currencySelect) {
      case Currencies.EUR:
        return this.rates.EUR * this.lite;
      case Currencies.USD:
        return this.rates.USD * this.lite;
      case Currencies.ARS:
        return this.rates.ARS * this.lite;
      case Currencies.MXN:
        return this.rates.MXN * this.lite;
      case Currencies.GBP:
        return this.rates.GBP * this.lite;
      case Currencies.BRL:
        return this.rates.BRL * this.lite;
      case Currencies.INR:
        return this.rates.INR * this.lite;
      case Currencies.CAD:
        return this.rates.CAD * this.lite;
      case Currencies.RUB:
        return this.rates.RUB * this.lite;
      case Currencies.SGD:
        return this.rates.SGD * this.lite;
      case Currencies.RON:
        return this.rates.RON * this.lite;
      case Currencies.JPY:
        return this.rates.JPY * this.lite;
      case Currencies.MYR:
        return this.rates.MYR * this.lite;
      case Currencies.CLP:
        return this.rates.CLP * this.lite;
      case Currencies.PEN:
        return this.rates.PEN * this.lite;
      case Currencies.MAD:
        return this.rates.MAD * this.lite;
      case Currencies.AUD:
        return this.rates.AUD * this.lite;
      case Currencies.CHF:
        return this.rates.CHF * this.lite;
    }
  }

  getPremiumValue(): number {
    switch (this.currencySelect) {
      case Currencies.EUR:
        return this.rates.EUR * this.premium;
      case Currencies.USD:
        return this.rates.USD * this.premium;
      case Currencies.ARS:
        return this.rates.ARS * this.premium;
      case Currencies.MXN:
        return this.rates.MXN * this.premium;
      case Currencies.GBP:
        return this.rates.GBP * this.premium;
      case Currencies.BRL:
        return this.rates.BRL * this.premium;
      case Currencies.INR:
        return this.rates.INR * this.premium;
      case Currencies.CAD:
        return this.rates.CAD * this.premium;
      case Currencies.RUB:
        return this.rates.RUB * this.premium;
      case Currencies.SGD:
        return this.rates.SGD * this.premium;
      case Currencies.RON:
        return this.rates.RON * this.premium;
      case Currencies.JPY:
        return this.rates.JPY * this.premium;
      case Currencies.MYR:
        return this.rates.MYR * this.premium;
      case Currencies.CLP:
        return this.rates.CLP * this.premium;
      case Currencies.PEN:
        return this.rates.PEN * this.premium;
      case Currencies.MAD:
        return this.rates.MAD * this.premium;
      case Currencies.AUD:
        return this.rates.AUD * this.premium;
      case Currencies.CHF:
        return this.rates.CHF * this.premium;
    }
  }

  getSymbol(): string {
    switch (this.currencySelect) {
      case Currencies.EUR:
        return "€";
      case Currencies.GBP:
        return "£";
      case Currencies.BRL:
        return "R$";
      case Currencies.INR:
        return "₹";
      case Currencies.RUB:
        return "₽"
      case Currencies.RON:
        return "lei";
      case Currencies.JPY:
        return "¥";
      case Currencies.MYR:
        return "RM";
      case Currencies.PEN:
        return "S/."
      case Currencies.MAD:
        return ".د.م";
      case Currencies.CHF:
        return "CHF";
      default:
        return "$";
    }
  }
}

export class Rates {
  AED: number = 0;
  AFN: number = 0;
  ALL: number = 0;
  AMD: number = 0;
  ANG: number = 0;
  AOA: number = 0;
  ARS: number = 0;
  AUD: number = 0;
  AWG: number = 0;
  AZN: number = 0;
  BAM: number = 0;
  BBD: number = 0;
  BDT: number = 0;
  BGN: number = 0;
  BHD: number = 0;
  BIF: number = 0;
  BMD: number = 0;
  BND: number = 0;
  BOB: number = 0;
  BRL: number = 0;
  BSD: number = 0;
  BTC: number = 0;
  BTN: number = 0;
  BWP: number = 0;
  BYN: number = 0;
  BYR: number = 0;
  BZD: number = 0;
  CAD: number = 0;
  CDF: number = 0;
  CHF: number = 0;
  CLF: number = 0;
  CLP: number = 0;
  CNY: number = 0;
  COP: number = 0;
  CRC: number = 0;
  CUC: number = 0;
  CUP: number = 0;
  CVE: number = 0;
  CZK: number = 0;
  DJF: number = 0;
  DKK: number = 0;
  DOP: number = 0;
  DZD: number = 0;
  EGP: number = 0;
  ERN: number = 0;
  ETB: number = 0;
  EUR: number = 0;
  FJD: number = 0;
  FKP: number = 0;
  GBP: number = 0;
  GEL: number = 0;
  GGP: number = 0;
  GHS: number = 0;
  GIP: number = 0;
  GMD: number = 0;
  GNF: number = 0;
  GTQ: number = 0;
  GYD: number = 0;
  HKD: number = 0;
  HNL: number = 0;
  HRK: number = 0;
  HTG: number = 0;
  HUF: number = 0;
  IDR: number = 0;
  ILS: number = 0;
  IMP: number = 0;
  INR: number = 0;
  IQD: number = 0;
  IRR: number = 0;
  ISK: number = 0;
  JEP: number = 0;
  JMD: number = 0;
  JOD: number = 0;
  JPY: number = 0;
  KES: number = 0;
  KGS: number = 0;
  KHR: number = 0;
  KMF: number = 0;
  KPW: number = 0;
  KRW: number = 0;
  KWD: number = 0;
  KYD: number = 0;
  KZT: number = 0;
  LAK: number = 0;
  LBP: number = 0;
  LKR: number = 0;
  LRD: number = 0;
  LSL: number = 0;
  LTL: number = 0;
  LVL: number = 0;
  LYD: number = 0;
  MAD: number = 0;
  MDL: number = 0;
  MGA: number = 0;
  MKD: number = 0;
  MMK: number = 0;
  MNT: number = 0;
  MOP: number = 0;
  MRO: number = 0;
  MUR: number = 0;
  MVR: number = 0;
  MWK: number = 0;
  MXN: number = 0;
  MYR: number = 0;
  MZN: number = 0;
  NAD: number = 0;
  NGN: number = 0;
  NIO: number = 0;
  NOK: number = 0;
  NPR: number = 0;
  NZD: number = 0;
  OMR: number = 0;
  PAB: number = 0;
  PEN: number = 0;
  PGK: number = 0;
  PHP: number = 0;
  PKR: number = 0;
  PLN: number = 0;
  PYG: number = 0;
  QAR: number = 0;
  RON: number = 0;
  RSD: number = 0;
  RUB: number = 0;
  RWF: number = 0;
  SAR: number = 0;
  SBD: number = 0;
  SCR: number = 0;
  SDG: number = 0;
  SEK: number = 0;
  SGD: number = 0;
  SHP: number = 0;
  SLL: number = 0;
  SOS: number = 0;
  SRD: number = 0;
  STD: number = 0;
  SVC: number = 0;
  SYP: number = 0;
  SZL: number = 0;
  THB: number = 0;
  TJS: number = 0;
  TMT: number = 0;
  TND: number = 0;
  TOP: number = 0;
  TRY: number = 0;
  TTD: number = 0;
  TWD: number = 0;
  TZS: number = 0;
  UAH: number = 0;
  UGX: number = 0;
  USD: number = 0;
  UYU: number = 0;
  UZS: number = 0;
  VEF: number = 0;
  VND: number = 0;
  VUV: number = 0;
  WST: number = 0;
  XAF: number = 0;
  XAG: number = 0;
  XAU: number = 0;
  XCD: number = 0;
  XDR: number = 0;
  XOF: number = 0;
  XPF: number = 0;
  YER: number = 0;
  ZAR: number = 0;
  ZMK: number = 0;
  ZMW: number = 0;
  ZWL: number = 0;
}
