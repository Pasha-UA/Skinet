import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IUser } from '../shared/models/user';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private isAdminSource = new ReplaySubject<boolean>(1);
  isAdmin$ = this.isAdminSource.asObservable();
  tokenExpiresIn = environment.authTokenExpiresIn;

  constructor(private http: HttpClient, private router: Router, private cookieService: CookieService) { }

  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'account', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          // localStorage.setItem('token', user.token);
          // this.cookieService.set('token', `Bearer ${user.token}`, undefined, '/', undefined, true, 'Strict');
          this.cookieService.set('token', `${user.token}`, this.authTokenExpireDate(user.token), '/', undefined, true, 'Strict');
          this.currentUserSource.next(user);
          this.isAdminSource.next(this.isAdmin(user.token));
        }
      })
    );
  }

  login(values: any): Observable<IUser> {
    return this.http.post(this.baseUrl + 'account/login', values).pipe(
      map((user: IUser) => {
        if (!user.emailConfirmationRequired) {
          // localStorage.setItem('token', user.token);
          // this.cookieService.set('token', `Bearer ${user.token}`, undefined, '/', undefined, true, 'Strict');
          this.cookieService.set('token', `${user.token}`, this.authTokenExpireDate(user.token), '/', undefined, true, 'Strict');
          this.currentUserSource.next(user);
          this.isAdminSource.next(this.isAdmin(user.token));
        }
        else {
          console.log('Email is not activated');
          this.currentUserSource.next(user);
        }
        return user;
      })
    );
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(
      map((user: IUser) => {
        if (!user.emailConfirmationRequired) {
          // localStorage.setItem('token', user.token);
          // this.cookieService.set('token', `Bearer ${user.token}`, undefined, '/', undefined, true, 'Strict');
          this.cookieService.set('token', `${user.token}`, this.authTokenExpireDate(user.token), '/', undefined, true, 'Strict');
          this.isAdminSource.next(this.isAdmin(user.token));
        }
        else {
          console.log('Email is not activated');
        }
        return user;
      }));
  }


  emailConfirmation(email: string) {
    return this.http.get(this.baseUrl + 'account/emailconfirmation?email=' + email);
  }

  emailConfirm(values: any){
    return this.http.post(this.baseUrl + 'account/emailconfirm', values).pipe(
      map((user: IUser) => {
        if (user) {
          // localStorage.setItem('token', user.token);
          this.cookieService.set('token', `${user.token}`, this.authTokenExpireDate(user.token), '/', undefined, true, 'Strict');
          this.currentUserSource.next(user);
          this.isAdminSource.next(this.isAdmin(user.token));
        }
      }));  
  }

  isAdmin(token: string): boolean {
    if (token) {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      if (decodedToken.role.indexOf('Admin') > -1) {
        return true;
      }
    }
  }

  authTokenExpireDate(token: string) : Date {
    if (token) {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      const expiresIn = new Date( decodedToken.exp)
      const expDate = new Date(decodedToken.exp * 1000);
      console.log(expDate);
      return expDate;
    }
  }

  logout() {
    // localStorage.removeItem('token');
    this.cookieService.delete('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailexists?email=' + email);
  }

  getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }

}
