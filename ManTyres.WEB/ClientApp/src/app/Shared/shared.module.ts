import { NgModule } from "@angular/core";

import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MaterialModule } from './material.module';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { MaterialElevationDirective } from "./Directives/mat-elevation.directive";
import { ImageViewerComponent } from './Components/ImageViewer/image-viewer.component';
import { NotFoundComponent } from "./Components/not-found/not-found.component";
import { ForbiddenComponent } from './Components/Forbidden/forbidden.component';
import { ErrorComponent } from "./Components/error/error.component";
import { SetupComponent } from "./Components/Setup/setup.component";
import { NgChartsModule } from 'ng2-charts';
import { LoaderComponent } from './Components/Loader/loader.component';
import { PricingComponent } from "./Components/pricing/pricing.component";
import { ToastrModule } from "ngx-toastr";
import { HeaderComponent } from "./Components/Header/Header.component";
import { SideNavComponent } from "./Components/SideNav/SideNav.component";
import { RecaptchaModule } from "ng-recaptcha";

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    NotFoundComponent,
    ForbiddenComponent,
    ErrorComponent,
    MaterialElevationDirective,
    ImageViewerComponent,
    SetupComponent,
    LoaderComponent,
    PricingComponent,
    HeaderComponent,
    SideNavComponent
  ],
  imports: [
    ToastrModule.forRoot(),
    RouterModule,
    MaterialModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMatSelectSearchModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    NgChartsModule,
    RecaptchaModule,
  ],
  exports: [
    ErrorComponent,
    ForbiddenComponent,
    ToastrModule,
    NotFoundComponent,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MaterialElevationDirective,
    ImageViewerComponent,
    NgxMatSelectSearchModule,
    TranslateModule,
    SetupComponent,
    NgChartsModule,
    LoaderComponent,
    HeaderComponent,
    SideNavComponent,
    RecaptchaModule,
  ],
  providers: []
})
export class SharedModule {
  constructor(public translate: TranslateService) {
    this.translate.addLangs(["en", "fr", "es", "it", "de"]);
    const browserLang = this.translate.getBrowserLang();
    var lang = localStorage.getItem("lang");
    if (lang && lang != "undefined") {
      this.translate.setDefaultLang(lang);
      this.translate.use(lang);
    } else if (browserLang) {
      this.translate.use(browserLang.match(/en|fr|es|it|de/) ? browserLang : "en");
    } else {
      this.translate.setDefaultLang("en");
    }
  }
}
