import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginModel } from 'src/app/Account/Models/login.model';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { UserService } from '../../Services/user.service';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';

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
    email: ['', Validators.required],
    password: ['', Validators.required],
  });
  hide = true;
  returnUrl: string = '';
  caricamento = false;
  get getLoginControl() { return this.loginForm.controls; }

  user: any;
  loggedIn: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService,
    private authService: SocialAuthService,
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
    this.authService.authState.subscribe((user) => {
      this.loginWithGoogle(user);
      this.user = user;
      this.loggedIn = (user != null);
    }, err => this.loginWithGoogle(err));
  }

  loginWithGoogle(user: any) {
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
  }

  resolved(captchaResponse: string) {
    console.log(`Resolved captcha with response: ${captchaResponse}`);
  }

  ngOnInit() {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    /*
        if (this.userService.isLoggedIn()) {
          const token = this.userService.getToken();
          this.userService.setToken(token);
          if (token != null) {
            const payLoad = JSON.parse(window.atob(token.split('.')[1]));
            const username = payLoad.unique_name.charAt(0).toUpperCase() + payLoad.unique_name.slice(1);
            this.userService.yes();
            this.toastr.info('Bentornato! ' + username);
          }
        }
        this.subscribers.push(this.userService.Token.subscribe(token => {
          if (token != null) { this.router.navigate(['/']); }
        }));
    
        this.loginForm = this.formBuilder.group({
          email: ['', Validators.required],
          password: ['', Validators.required],
        });*/
  }

  /**** onSubmit della form ****/
  onSubmit() {
    // si ferma se il modulo non è valido
    if (this.loginForm.invalid) {
      return;
    }
    this.userService.setSession('test');
    this.router.navigate(['']);
    /* Login
    if (this.loginForm.valid) {
      const email = this.loginForm.get('email').value;
      // inizio
      this.caricamento = true;
      this.subscribers.push(this.userService.subscribeEmail(email).subscribe(
        (res: any) => {
          this.userService.setSession(email);
          this.router.navigate(['']);

          //this.userService.setToken(res.token);
          //var payLoad = JSON.parse(window.atob(res.token.split('.')[1]));
          //var username = payLoad.unique_name.charAt(0).toUpperCase() + payLoad.unique_name.slice(1);
          //this.userService.yes();
          //this.toastr.info('Bentornato! ' + username);
          this.caricamento = false;
        },
        error => {
          if (error.status == 400) {
            this.toastr.error('Autenticazione fallita');
          }
          if (error.status == 500) {
            this.router.navigate(['/errore']);
          }
          this.caricamento = false;
        }
      ));
    }
    else {
      this.loginForm.get('username').markAsDirty();
      this.loginForm.get('password').markAsDirty();
    }*/
  }

  signInWithGoogle(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    this.authService.authState.subscribe((user) => {
      this.user = user;
      this.loggedIn = (user != null);
    });
  }

}
