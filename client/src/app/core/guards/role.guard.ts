import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Route, RouterStateSnapshot } from '@angular/router';
import { map, Observable, of } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard {

  constructor(private accountSercice: AccountService, private route: Route) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    // TODO role-based guard

    return of(true);
  }

  // canActivate(
  //   route: ActivatedRouteSnapshot, 
  //   state: RouterStateSnapshot): Observable<boolean> {
  //   return this.accountSercice.canActivateProtectedRoutes$.pipe(
  //     map((canActivateProtectedRoutes: boolean) => {
  //       if (canActivateProtectedRoutes) {
  //         // role check only if route contain data.role
  //         // https://javascript.plainenglish.io/4-ways-to-check-whether-the-property-exists-in-a-javascript-object-20c2d96d8f6e
  //         if (!!route.data.role) {
  //           const routeRoles = route.data.role;
  //           //this.showToaster('Role guard', 'Require role ' + routeRoles);

  //           this.userProfile = this.authService.identityClaims;
  //           if (!!this.userProfile.role) {
  //             const userRoles = this.userProfile.role;
  //             //this.showToaster('Role guard', 'User profile role ' + userRoles);

  //             if (userRoles.includes(routeRoles)) {
  //               // user's roles contains route's role
  //               return true;
  //             } else {
  //               // toaster-display role user needs to have to access this route;
  //               this.showToaster('Access denied', 'You do not have role ' + routeRoles);
  //             }
  //           }
  //         }
  //       }
  //       return false;
  //     })
  //   );
  // }

}

