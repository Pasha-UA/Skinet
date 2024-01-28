import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Skinet';

  constructor(private basketService: BasketService, private accountService: AccountService, private cookieService: CookieService ) { }

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser()
  }

  loadCurrentUser() {
    // const token = localStorage.getItem('token');
    const token = this.cookieService.get('token')
    this.accountService.loadCurrentUser(token).subscribe({
      next: () => {
        console.log('loaded user');
      },
      error: e => {
        console.log(e);
      }
    });
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe(
        {
          next: () => {
            console.log('initialized basket' + basketId);
          },
          error: e => {
            console.log(e);
          }
        });
    }
  }


}
