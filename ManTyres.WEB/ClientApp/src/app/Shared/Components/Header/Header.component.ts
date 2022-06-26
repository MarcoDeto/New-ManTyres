import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { UserService } from 'src/app/Account/Services/user.service';
import Swal from 'sweetalert2';
import { LanguageNames } from '../../Models/languages.enum';

@Component({
  selector: 'app-header',
  templateUrl: './Header.component.html',
  styleUrls: ['./Header.component.scss']
})
export class HeaderComponent implements OnInit {
  activeLang: LanguageNames = LanguageNames.en;
  @Input() open: boolean = false;
  @Input() hideHeader: boolean = false;
  @Output() sidenavChanged = new EventEmitter<boolean>();


  constructor(
    public userService: UserService,
    private router: Router,
    public translate: TranslateService,
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

  changeLang(lang: string): void {
    this.activeLang = this.getLanguage(lang);
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
    localStorage.setItem("lang", lang);
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
