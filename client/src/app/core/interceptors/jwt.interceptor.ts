import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CookieService } from "ngx-cookie-service";
import { Observable } from "rxjs";

@Injectable ()
export class JwtInterceptor implements HttpInterceptor{

    constructor(private cookieService: CookieService) { }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // const token = localStorage.getItem('token');
        const token = this.cookieService.get('token')
        if (token){
            req = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }
        return next.handle(req);
    }
}
