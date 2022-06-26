import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './Shared/shared.module';
import { AccountModule } from './Account/Account.module';
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { environment } from '../environments/environment';
import { provideAuth, getAuth } from '@angular/fire/auth';
import { SETTINGS as AUTH_SETTINGS, TENANT_ID } from '@angular/fire/compat/auth';
import { USE_DEVICE_LANGUAGE } from '@angular/fire/compat/auth';
import { LANGUAGE_CODE } from '@angular/fire/compat/auth';
import { PERSISTENCE } from '@angular/fire/compat/auth';
import { MaterialModule } from './Shared/material.module';
import { PricingComponent } from './Shared/Components/pricing/pricing.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AccountModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: 'price', component: PricingComponent },
      { path: 'account', loadChildren: () => import('./Account/Account.module').then(mod => mod.AccountModule)},
      //{ path: 'account', loadChildren: () => import('./Account/Account.module').then(mod => mod.AccountModule)},
    ]),
    BrowserAnimationsModule,
    SharedModule,
    MaterialModule,
    provideFirebaseApp(() => initializeApp(environment.firebase)),
    provideAuth(() => getAuth()),
  ],
  providers: [
    { provide: AUTH_SETTINGS, useValue: { appVerificationDisabledForTesting: true } },
    { provide: USE_DEVICE_LANGUAGE, useValue: true },
    { provide: LANGUAGE_CODE, useValue: 'it' },
    { provide: PERSISTENCE, useValue: 'session' },
    { provide: TENANT_ID, useValue: 'tenant-id-app-one' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
