import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Language } from 'src/app/Shared/Models/language.model';
import { Response } from 'src/app/Shared/Models/response.model';
import { ConnectionInfoService } from 'src/app/Shared/Services/connectionInfo.service';

@Component({
  selector: 'app-local-settings',
  templateUrl: './local-settings.component.html',
  styleUrls: ['./local-settings.component.scss']
})
export class LocalSettingsComponent implements OnInit {

  languages: Language[] = [];

  constructor(
    public dialogRef: MatDialogRef<LocalSettingsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any | undefined,
    public languageService: ConnectionInfoService,
  ) { }

  ngOnInit() {
    this.getLanguages();
  }

  closeDialog() {
    this.dialogRef.close(null);
  }

  getLanguages() {
    this.languageService.GetLanguages().subscribe(
      (res: Response) => {
        let langs = [...res.content];
        const country_code: string = this.languageService.getCountryCode()!;
        const firstLanguage: Language = res.content.find((x: any) => x.code.includes(country_code))!;
        const firstLanguageindex: number = res.content.findIndex((x: any) => x.code.includes(country_code))!;
        this.languages = [];
        this.languages.push(firstLanguage);
        langs.splice(firstLanguageindex, 1);
        this.languages = this.languages.concat(langs);
        //this.languages = [...res.content];
      }
    )
  }

  checkCountryCode(language: Language): boolean {
    var country_code = this.languageService.getCountryCode()!;
    if (language.code.includes(country_code)) {
      return true;
    }
    return false;
  }
}
