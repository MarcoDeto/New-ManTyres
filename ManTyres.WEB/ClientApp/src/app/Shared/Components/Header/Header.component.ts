import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserService } from 'src/app/Auth/Services/user.service';
import Swal from 'sweetalert2';
import { LanguageNames } from '../../Models/languages.enum';
import { LocalSettingsComponent } from './local-settings/local-settings.component';

@Component({
  selector: 'app-header',
  templateUrl: './Header.component.html',
  styleUrls: ['./Header.component.scss']
})
export class HeaderComponent implements OnInit {
  activeLang: LanguageNames = LanguageNames.en;
  @Input() open: boolean = false;
  @Output() sidenavChanged = new EventEmitter<boolean>();
  photoUrl = '';
  constructor(
    public userService: UserService,
    public router: Router,
    public translate: TranslateService,
    public dialog: MatDialog,
  ) { }

  get languageNames() {
    return LanguageNames;
  }

  ngOnInit() {
    const lang = localStorage.getItem("lang")!;
    this.activeLang = this.getLanguage(lang);
  }
  getWidth(): number {
    return window.innerWidth;
  }

  logout() {
    Swal.fire({
      title: 'Effettuare il Logout?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sì',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.value) {
        this.userService.logout();
        sessionStorage.clear();
        localStorage.clear();
        this.router.navigate(['account/login']);
      }
    });
  }

  changeSideNav() {
    this.open = !this.open;
    this.sidenavChanged.emit(this.open);
  }

  hideHeader(): boolean {
    if (
      this.router.url != '/account/login' &&
      this.router.url != '/account/register') {
      return true;
    }
    return false;
  }

  changeLang(lang: string): void {
    this.activeLang = this.getLanguage(lang);
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
    localStorage.setItem("lang", lang);
  }

  openLocalSettings() {
    const dialogRef = this.dialog.open(LocalSettingsComponent, {
      width: '80%',
      data: { }
    });
    dialogRef.beforeClosed().subscribe((result: any) => { this.ngOnInit(); });
    dialogRef.afterClosed().subscribe((result: any) => { this.ngOnInit(); });
  }

  getLanguage(lang: string): LanguageNames {
    switch (lang) {
      case "cn":
        return LanguageNames.cn;
      case "dk":
        return LanguageNames.dk;
      case "de":
        return LanguageNames.de;
      case "es":
        return LanguageNames.es;
      case "fr":
        return LanguageNames.fr;
      case "in":
        return LanguageNames.in;
      case "it":
        return LanguageNames.it;
      case "jp":
        return LanguageNames.jp;
      case "no":
        return LanguageNames.no;
      case "pl":
        return LanguageNames.pl;
      case "pt":
        return LanguageNames.pt;
      case "ro":
        return LanguageNames.ro;
      case "ru":
        return LanguageNames.ru;
      case "se":
        return LanguageNames.se;
      case "tr":
        return LanguageNames.tr;
      case "vn":
        return LanguageNames.vn;
      default:
        return LanguageNames.en;
    }
  }
}
