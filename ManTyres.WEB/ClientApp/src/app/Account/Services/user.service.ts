import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject, Subscriber } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { UserPassword, UserName, User } from '../../Shared/Models/user.model';
import { Response } from '../../Shared/Models/response.model';
import { catchError } from 'rxjs/operators';
import { LoginModel } from '../Models/login.model';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private _userId: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public userId: Observable<string> = this._userId.asObservable();

  private _userRole: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public userRole: Observable<string> = this._userRole.asObservable();

  private _userUsername: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public userUsername: Observable<string> = this._userUsername.asObservable();

  private _expiration: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public expiration: Observable<string> = this._expiration.asObservable();

  private token: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public Token: Observable<string> = this.token.asObservable();

  private isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public IsAuthenticated: Observable<boolean> = this.isAuthenticated.asObservable();

  private _hiddenPopUpPdf: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public hiddenPopUpPdf: Observable<boolean> = this._hiddenPopUpPdf.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  createAccount(request: User) {
    return this.http.post(environment.BASE_URL + 'UserManager/Create', request);
  }

  roleMatch(allowedRoles: any): boolean {
    var payLoad = null;
    var token = this.getToken();
    if (token != null)
      payLoad = JSON.parse(window.atob(token.split('.')[1]))
    if (payLoad) {
      var userRole = payLoad.role;
      allowedRoles.forEach((element: any) => {
        if (userRole.toLowerCase() == element.toLowerCase()) {
          return true;
        }
        return false;
      });
    }
    return false;
  }

  login(data: LoginModel) {
    return this.http.post<LoginModel>(environment.login, data);
  }

  subscribeEmail(email: string) {
    return this.http.get(environment.firstConnection + '/subscribeEmail?email=' + email);
  }

  profile(currentUserName: string): Observable<Response> {
    return this.http.get<Response>(environment.profile + '?username=' + currentUserName);
  }

  checkCurrentPassword(data: UserPassword): Observable<Response> {
    return this.http.post<Response>(environment.password + '/CheckCurrentPassword', data);
  }

  changePassword(data: UserPassword): Observable<Response> {
    return this.http.post<Response>(environment.password + '/ChangePassword', data);
  }

  missedPassword(email: string) {
    return this.http.get(environment.password + '/MissedPassword/' + email);
  }

  /*existsEmail(email) {
    return this.http.get(environment.existsEmail + '/' + email);
  }*/

  setToken(data: any) {
    this.token.next(data);
    let payLoad = JSON.parse(window.atob(data.split('.')[1]));
    debugger;
    this._userId.next(payLoad.UserID);
    this._userUsername.next(payLoad.unique_name);
    this._userRole.next(payLoad.role);
    this._expiration.next(data.expiration);
  }

  setSession(data: any) {
    //localStorage.setItem('email', email);
    let payLoad = JSON.parse(window.atob(data.token.split('.')[1]))
    localStorage.setItem('token', data.token);
    localStorage.setItem('UserID', payLoad.UserID);
    localStorage.setItem('Username', payLoad.unique_name);
    localStorage.setItem('UserRole', payLoad.role);
    localStorage.setItem("expiration", JSON.stringify(data.expiration.valueOf()));
  }

  setGoogleSession(data: any) {
    //localStorage.setItem('email', email);
    //let payLoad = JSON.parse(window.atob(data.authToken.split('.')[1]));
    //let payLoad2 = JSON.parse(window.atob(data.idToken.split('.')[1]));
    localStorage.setItem('token', data.authToken);
    localStorage.setItem('UserID', data.idToken);
    localStorage.setItem('Username', data.name);
    localStorage.setItem('UserRole', data.role);
    localStorage.setItem("expiration", JSON.stringify(data.expiration.valueOf()));
  }

  getExpiration() {
    const expiration = localStorage.getItem("expiration")!;
    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }

  getEmail() { return localStorage.getItem("email"); }
  getToken() { return localStorage.getItem("token"); }
  getUserID() { return localStorage.getItem("UserID"); }
  getUsername() { return localStorage.getItem("Username"); }
  getUserRole() { return localStorage.getItem("UserRole"); }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("UserID");
    localStorage.removeItem("Username");
    localStorage.removeItem("UserRole");
    localStorage.removeItem("expiration");
  }

  public isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  removeToken() { this.token.next(''); }

  yes() { this.isAuthenticated.next(true); }
  no() { this.isAuthenticated.next(false); }

  getHiddenPopUpPdf() { return localStorage.getItem("hiddenPopUpPdf"); }

  hidePopUpPdf() {
    this._hiddenPopUpPdf.next(true);
    localStorage.setItem('hiddenPopUpPdf', 'true');
  }

  showPopUpPdf() {
    this._hiddenPopUpPdf.next(false);
    localStorage.removeItem("hiddenPopUpPdf");
  }
}

export class Iscritto {
  constructor(
    public email: string
  ) { }
}

