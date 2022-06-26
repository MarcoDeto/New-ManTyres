import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginModel } from 'src/app/Account/Models/login.model';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { GoogleLoginProvider, FacebookLoginProvider, SocialAuthService } from 'angularx-social-login';
import { TranslateService } from '@ngx-translate/core';
import { UserRole } from 'src/app/Shared/Models/enums';
import { User } from 'src/app/Shared/Models/user.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  title = 'Login';
  subscribers: Subscription[] = [];
  loginData: LoginModel = { email: '', password: '' };
  loginForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  });
  hide: boolean = true;
  caricamento: boolean = false;
  get getLoginControl() { return this.loginForm.controls; }

  logohover: boolean = false;
  emailaddress: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService,
    private authService: SocialAuthService,
    private translate: TranslateService
  ) {
    this.subscribers = [];
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  openGooglePopup(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    this.socialLogin();
  }

  openFacebookPopup(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
    this.socialLogin();
  }

  socialLogin() {
    this.subscribers.push(this.authService.authState.subscribe({
      next: (user: any) => {
        this.login(user, true);
      },
      error: (error: any) => {
        this.login(error, true);
      }
    }));
  }

  login(user: any, fromSocial: boolean) {
    var login: LoginModel = {
      password: user.id,
      email: user.email
    }
    this.userService.login(login).subscribe({
      next: (res: any) => {
        this.userService.setSession(res);
        this.toastr.success(this.translate.instant('SUCCESS_WELCOME'));
        this.router.navigate(['home']);
      },
      error: (err: any) => {
        if (fromSocial == false) {
          this.toastr.error(this.translate.instant(err.message));
        } else if (err.error && err.error.code == 404) {
          this.SignUpWithSocial(user);
        }
      }
    });
  }

  resolved(captchaResponse: string) {
    console.log(`Resolved captcha with response: ${captchaResponse}`);
  }

  ngOnInit() {
    localStorage.clear();
    sessionStorage.clear();

    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
    this.authService.signOut(true);
  }

  onSubmit() {
    this.loginForm.markAllAsTouched();
    if (this.loginForm.invalid) { return; }

    this.caricamento = true;
    this.subscribers.push(this.userService.login(this.loginForm.value).subscribe({
      next: (res: any) => {
        console.log(res);
        this.userService.setSession(res);
        this.router.navigate(['']);
        var payLoad = JSON.parse(window.atob(res.token.split('.')[1]));
        var username = payLoad.unique_name.charAt(0).toUpperCase() + payLoad.unique_name.slice(1);
        this.userService.yes();
        this.toastr.info('Bentornato! ' + username);
      },
      error: (error: any) => {
        if (error.status == 400) {
          this.toastr.error(this.translate.instant(error.error.message));
        }
        if (error.status == 500) {
          this.router.navigate(['/errore']);
        }
        this.caricamento = false;
      }
    }));
  }

  SignUpWithSocial(user: any) {
    const toAdd: User = {
      city: null,
      companyName: null,
      concurrencyStamp: null,
      country: null,
      cultureInfo: null,
      email: user.email,
      emailConfirmed: true,
      firstName: user.firstName,
      id: null,
      isDeleted: false,
      lastName: user.lastName,
      passwordHash: user.id,
      phoneNumber: null,
      phoneNumberConfirmed: false,
      photoUrl: user.photoUrl,
      provider: user.provider,
      securityStamp: null,
      street: null,
      twoFactorEnabled: false,
      userName: user.name,
      userRole: UserRole.Administrator,
      website: null,
      zipcode: null,
      socialUserId: null,
      createdAt: new Date(),
      updatedAt: new Date()
    }
    this.subscribers.push(this.userService.createAccount(toAdd).subscribe({
      next: (boolRes: any) => {
        this.toastr.success(this.translate.instant('SUCCESS_WELCOME') + ' ' + toAdd.firstName);
        var login: LoginModel = {
          password: user.id,
          email: user.email
        }
        this.userService.login(login).subscribe({
          next: (res: any) => {
            this.userService.setSession(res);
            this.userService.setUser(toAdd);
            this.router.navigate(['account/register/profile']);
          }
        });
      },
      error: (error: any) => {
        if (error.error.code == 409) {
          var login: LoginModel = {
            password: user.id,
            email: user.email
          }
          this.userService.login(login).subscribe(
            res => {
              this.userService.setSession(res);
              this.router.navigate(['home']);
            }
          );
        } else {
          this.toastr.error(error);
        }
      }
    }));
  }
}
