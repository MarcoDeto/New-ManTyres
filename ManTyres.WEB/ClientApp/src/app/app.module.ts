import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './Shared/shared.module';
import { AccountModule } from './Auth/Account.module';
import { initializeApp, provideFirebaseApp } from '@angular/fire/app';
import { environment } from '../environments/environment';
import { provideAuth, getAuth } from '@angular/fire/auth';
import { SETTINGS as AUTH_SETTINGS, TENANT_ID } from '@angular/fire/compat/auth';
import { USE_DEVICE_LANGUAGE } from '@angular/fire/compat/auth';
import { LANGUAGE_CODE } from '@angular/fire/compat/auth';
import { PERSISTENCE } from '@angular/fire/compat/auth';
import { MaterialModule } from './Shared/material.module';
import { PricingComponent } from './Shared/Components/pricing/pricing.component';
import { AdminModule } from './Admin/admin.module';
import { NotFoundComponent } from './Shared/Components/not-found/not-found.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalSettingsComponent } from './Shared/Components/Header/local-settings/local-settings.component';
import { ForbiddenComponent } from './Shared/Components/Forbidden/forbidden.component';
import { ErrorComponent } from './Shared/Components/error/error.component';
import { SetupComponent } from './Shared/Components/Setup/setup.component';
import { HomeComponent } from './home/home.component';
import { PlaceCardComponent } from './home/place-card/place-card.component';
import { PlaceDetailComponent } from './home/place-detail/place-detail.component';
import { UserModule } from './User/user.module';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PlaceCardComponent,
    PlaceDetailComponent,
    LocalSettingsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    AccountModule,
    NgbModule,
    SharedModule,
    MaterialModule,
    AdminModule,
    UserModule,
    provideFirebaseApp(() => initializeApp(environment.firebase)),
    provideAuth(() => getAuth()),
    RouterModule.forRoot([
      { path: '', component: HomeComponent },
      { path: 'price', component: PricingComponent },
      { path: 'account', loadChildren: () => import('./Auth/Account.module').then(mod => mod.AccountModule)},
      { path: 'admin', loadChildren: () => import('./Admin/admin.module').then(mod => mod.AdminModule)},
      { path: 'user', loadChildren: () => import('./User/user.module').then(mod => mod.UserModule)},
      { path: 'place/:id', component: PlaceDetailComponent },
      
      { path: 'forbidden', component: ForbiddenComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'setup', component: SetupComponent },
      
      { path: '**', component: NotFoundComponent }
    ]),
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
