import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { UserPassword, User } from '../../Shared/Models/user.model';
import { Response } from '../../Shared/Models/response.model';
import { LoginModel } from '../Models/login.model';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private loading: boolean = false;
  public userShared: User | undefined;

  private _user: BehaviorSubject<User|null> = new BehaviorSubject<User|null>(null);
  public user: Observable<User|null> = this._user.asObservable();

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

  private _hiddenPopUpPdf: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  public hiddenPopUpPdf: Observable<boolean> = this._hiddenPopUpPdf.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  createAccount(request: User): Observable<Response> {
    return this.http.post<Response>(environment.BASE_URL + 'UserManager/Create', request);
  }

  login(data: LoginModel): Observable<Response> {
    return this.http.post<Response>(environment.login, data);
  }

  getById(id: string): Observable<Response> {
    return this.http.get<Response>(environment.utente + 'GetById/' + id);
  }

  putUser(bodyReq: User): Observable<Response> {
    return this.http.put<Response>(environment.utente + 'Update', bodyReq);
  }

  subscribeEmail(email: string) {
    return this.http.get(environment.firstConnection + '/subscribeEmail?email=' + email);
  }

  /*profile(currentUserName: string): Observable<Response> {
    return this.http.get<Response>(environment.profile + '?username=' + currentUserName);
  }*/

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
    this._userId.next(payLoad.UserID);
    this._userUsername.next(payLoad.UserName);
    this._userRole.next(payLoad.role);
    this._expiration.next(data.expiration);
  }

  setUser(data: User) { this._user.next(data); this.userShared = data; }

  setSession(data: any) {
    if (data) {
      this.setUser(data.user);
      if (data.token) {
        const payLoad: any = JSON.parse(window.atob(data.token.split('.')[1]));
        localStorage.setItem('token', data.token);
        localStorage.setItem('UserID', payLoad.UserID);
        localStorage.setItem('Username', payLoad.UserName);
        localStorage.setItem('UserRole', payLoad.role);
      }
      if (data.expiration) {
        localStorage.setItem('expiration', JSON.stringify(data.expiration.valueOf()));
      }
    }
  }

  getEmail() { return localStorage.getItem('email'); }
  getToken() { return localStorage.getItem('token'); }
  getUserID() { return localStorage.getItem('UserID'); }
  getUsername() { return localStorage.getItem('Username'); }
  getUserRole() { return localStorage.getItem('UserRole'); }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('UserID');
    localStorage.removeItem('Username');
    localStorage.removeItem('UserRole');
    localStorage.removeItem('expiration');
    this.userShared = undefined;
  }

  public isLoggedIn() {
    var result = moment().isBefore(this.getExpiration());
    if (result && this.loading == false) {
      var userID = this.getUserID();
      if (userID && !this.userShared) {
        this.loading = true;
        this.getById(userID).subscribe(
          res => {
            this.userShared = res.content;
            this.loading = false;
          }
        );
      }
    }
    return result;
  }

  getExpiration() {
    const expiration = localStorage.getItem('expiration')!;
    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }

  removeToken() { this.token.next(''); }

  getHiddenPopUpPdf() { return localStorage.getItem('hiddenPopUpPdf'); }

  hidePopUpPdf() {
    this._hiddenPopUpPdf.next(true);
    localStorage.setItem('hiddenPopUpPdf', 'true');
  }

  showPopUpPdf() {
    this._hiddenPopUpPdf.next(false);
    localStorage.removeItem('hiddenPopUpPdf');
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
}