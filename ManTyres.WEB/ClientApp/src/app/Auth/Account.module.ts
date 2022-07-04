import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './Components/Login/login.component';
import { ProfileComponent } from './Components/Profile/profile.component';
import { SignupComponent } from './Components/Signup/signup.component';
import { RegisterProfileComponent } from './Components/Signup/RegisterProfile/RegisterProfile.component';
import { PasswordComponent } from './Components/password/password.component';
import { SharedModule } from '../Shared/shared.module';
import { ToastrModule } from 'ngx-toastr';
import { RecaptchaModule, RECAPTCHA_V3_SITE_KEY } from 'ng-recaptcha';
import { FacebookLoginProvider, GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from 'angularx-social-login';
import { environment } from 'src/environments/environment';

@NgModule({
  declarations: [
    LoginComponent,
    ProfileComponent,
    PasswordComponent,
    SignupComponent,
    RegisterProfileComponent
  ],
  imports: [
    RecaptchaModule,
    CommonModule,
    SocialLoginModule,
    SharedModule,
    ToastrModule.forRoot(),
    RouterModule.forRoot([
      {
        path: 'account',
        children: [
          // { path: '', redirectTo: 'login', pathMatch: 'full' },
          { path: 'login', component: LoginComponent },
          // TODO: Guard per reindirizzare alla pagina di compilazione profilo
          // nel caso in cui non sono stati inseriti tutti i dati necessari
          { path: 'profile', component: ProfileComponent },
          { path: 'password', component: PasswordComponent },
          { path: 'register', component: SignupComponent },
          { path: 'register/profile', component: RegisterProfileComponent },
          // { path: '**', component: NotFoundComponent }
        ],
      },
      { path: 'login', redirectTo: 'account' },
    ]),
  ],
  exports: [
    SocialLoginModule,
    RecaptchaModule,
  ],
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.GA_ID)
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider(environment.FB_ID)
          }
        ]
      } as SocialAuthServiceConfig,
    },
    { provide: RECAPTCHA_V3_SITE_KEY, useValue: '6Lc4fMQeAAAAANcjwZJE3DvxoifE_JUqMyQpGrGu' }
  ]
})
export class AccountModule { }
