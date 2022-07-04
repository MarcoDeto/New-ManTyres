import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoginModel } from 'src/app/Auth/Models/login.model';
import { UserService } from '../../Services/user.service';
import { SocialAuthService } from 'angularx-social-login';
import { FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';
import { User } from 'src/app/Shared/Models/user.model';
import { UserRole } from 'src/app/Shared/Models/enums';
import { Response } from 'src/app/Shared/Models/response.model';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit, OnDestroy {
  title = 'Sign up';
  subscribers: Subscription[] = [];

  loginData: LoginModel = { email: '', password: '' };
  signupForm: FormGroup = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]],
  });
  get getSignupControl() { return this.signupForm.controls; }

  hide = true;
  caricamento = false;
  emailSend = false;
  logohover: boolean = false;
  emailaddress: boolean = false;
  user: User = {
    city: null,
    companyName: null,
    concurrencyStamp: null,
    country: null,
    cultureInfo: null,
    email: null,
    emailConfirmed: false,
    firstName: null,
    imgProfile: null,
    lastName: null,
    passwordHash: null,
    phoneNumber: null,
    phoneNumberConfirmed: false,
    photoUrl: null,
    role: UserRole.Customer,
    securityStamp: null,
    socialUserId: null,
    street: null,
    twoFactorEnabled: false,
    userName: null,
    website: null,
    zipcode: null,
    googleId: null,
    facebookId: null,
    id: undefined,
    createdAt: new Date(),
    updatedAt: new Date(),
    isDeleted: false,
    taxCode: null,
    isBusiness: false,
    businessId: null,
    provider: null
  }

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService,
    public translate: TranslateService,
    private authService: SocialAuthService,
  ) { }

  ngOnInit() { }

  openGooglePopup(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    this.socialLogin();
  }

  openFacebookPopup(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
    this.socialLogin();
  }

  socialLogin() {
    this.subscribers.push(
      this.authService.authState.subscribe({
        next: (user: any) => {
          this.login(user, true);
        },
        error: (error: any) => {
          this.login(error, true);
        }
      })
    );
  }

  login(user: any, fromSocial: boolean) {
    var login: LoginModel = {
      password: user.id,
      email: user.email
    }
    this.subscribers.push(
      this.userService.login(login).subscribe({
        next: (res: any) => {
          this.userService.setSession(res);
          this.toastr.success(this.translate.instant('SUCCESS_WELCOME'));
          this.router.navigate(['']);
        },
        error: (err: any) => {
          if (fromSocial == false) {
            this.toastr.error(this.translate.instant(err.message));
          } else if (err.error && err.error.code == 404) {
            this.SignUpWithSocial(user);
          }
        }
      })
    );
  }

  SignUpWithSocial(user: any) {
    this.user.email = user.email;
    this.user.emailConfirmed = false;
    this.user.firstName = user.firstName;
    this.user.lastName = user.lastName;
    this.user.provider = user.provider;
    if (user.provider == 'GOOGLE') {
      this.user.googleId = user.id;
    } else if (user.provider == 'FACEBOOK') {
      this.user.facebookId = user.id;
    }
    this.user.photoUrl = user.photoUrl;
    this.user.passwordHash = user.id;
    this.user.userName = user.name;
    this.user.role = UserRole.Customer;

    this.subscribers.push(
      this.userService.createAccount(this.user).subscribe({
        next: (boolRes: Response) => {
          if (boolRes.content) {
            this.toastr.success(this.translate.instant('SUCCESS_WELCOME') + ' ' + this.user.firstName);
            var login: LoginModel = {
              password: user.id,
              email: user.email
            }
            this.userService.login(login).subscribe(
              (res: any) => {
                this.userService.setSession(res);
                this.userService.setUser(res.user);
                this.router.navigate(['account/register/profile']);
              }
            );
          }
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
                this.router.navigate(['']);
              }
            );
          } else {
            this.toastr.error(error);
          }
        }
      })
    );
  }

  onSubmit() {
    this.signupForm.markAllAsTouched();
    if (this.signupForm.invalid) { return; }

    this.caricamento = true;

    const toAdd: User = {
      city: null,
      companyName: null,
      concurrencyStamp: null,
      country: null,
      cultureInfo: null,
      email: this.signupForm.value.email,
      emailConfirmed: true,
      firstName: null,
      id: null,
      isDeleted: false,
      lastName: null,
      passwordHash: this.signupForm.value.password,
      phoneNumber: null,
      phoneNumberConfirmed: false,
      photoUrl: null,
      imgProfile: null,
      securityStamp: null,
      socialUserId: null,
      street: null,
      twoFactorEnabled: false,
      userName: this.signupForm.value.email,
      role: UserRole.Administrator,
      website: null,
      zipcode: null,
      createdAt: new Date(),
      updatedAt: new Date(),
      googleId: null,
      facebookId: null,
      taxCode: null,
      isBusiness: false,
      businessId: null,
      provider: null
    }
    this.subscribers.push(
      this.userService.createAccount(toAdd).subscribe({
        next: (res: any) => {
          this.router.navigate(['register/profile']);
        },
        error: (error: any) => {
          this.toastr.error(this.translate.instant(error.error.message));

          if (error.status == 500) {
            this.router.navigate(['/errore']);
          }
          this.caricamento = false;
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  sendEmail() { }

}
