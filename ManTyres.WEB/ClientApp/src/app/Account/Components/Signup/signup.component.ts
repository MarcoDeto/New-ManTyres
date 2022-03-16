import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoginModel } from 'src/app/Account/Models/login.model';
import { UserService } from '../../Services/user.service';
import { SocialAuthService } from "angularx-social-login";
import { FacebookLoginProvider, GoogleLoginProvider } from "angularx-social-login";
import { User } from 'src/app/Shared/Models/user.model';
import { UserRole } from 'src/app/Shared/Models/enums';

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
  hide = true;
  returnUrl: string = '';
  caricamento = false;
  emailSend = false;
  get getSignupControl() { return this.signupForm.controls; }
  user: any;
  loggedIn: any;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService,
    public translate: TranslateService,
    private authService: SocialAuthService,
  ) { }

  ngOnInit() {
    this.refreshToken();
  }

  signInWithGoogle(): void {
    debugger;
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    this.authService.authState.subscribe((user) => {
      this.SignUpWithGoogle(user);
      this.user = user;
      this.loggedIn = (user != null);
    }, err => this.SignUpWithGoogle(err));
  }

  SignUpWithGoogle(user: any) {
    if (user.provider == "GOOGLE" && user.idToken && user.authToken) {
      // userid del provider sarà salvata come password
      let toAdd: User = {
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
      }
      this.userService.createAccount(toAdd).subscribe(res => {
        this.toastr.success(this.translate.instant('SUCCESS_Welcome') + ' ' + toAdd.firstName);
      }, err => {
        if (err.error.code == 409) {
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
          this.toastr.error(this.translate.instant(err.message));
        }
      });
    }
  }

  signInWithFB(): void {
    //this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  signOut(): void {
    this.authService.signOut();
  }

  refreshToken(): void {
    this.authService.refreshAuthToken(GoogleLoginProvider.PROVIDER_ID);
    this.authService.authState.subscribe((user) => {
      this.user = user;
      this.loggedIn = (user != null);
    });
  }

  onSubmit() {
    this.userService.createAccount(this.signupForm.value).subscribe(
      res => {
        this.router.navigate(['register/profile']);
      }
    );
  }

  ngOnDestroy(): void {
    this.subscribers.forEach(s => s.unsubscribe());
    this.subscribers.splice(0);
    this.subscribers = [];
  }

  sendEmail() { }
}
