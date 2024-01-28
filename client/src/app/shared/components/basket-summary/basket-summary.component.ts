import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { ShopService } from 'src/app/shop/shop.service';
import { IBasket, IBasketItem } from '../../models/basket';
import { IProduct } from '../../models/product';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Input() isBasket: boolean = true;
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  basket$: Observable<IBasket>;
  products$: Observable<IProduct[]>; // products from db corresponding for basket items. need it to have current stocks of each product
  productStock$: Observable<[string, number]>;
  products: IProduct[];

  constructor(private basketService: BasketService, private shopService: ShopService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
    this.products$ = this.basket$.pipe(
      map(b => b.items.map(item => item.id)),
      switchMap(ids => this.shopService.getProductsArray(ids))
    )
    this.products$.subscribe({
      next:
        products => {
          this.products = products;
          console.log(this.products)
        }
    });
  }

  decrementItemQuantity(item: IBasketItem) {
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem) {
    const product = this.products.find(p => p.id === item.id)
    if (product.stock > item.quantity) this.increment.emit(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.remove.emit(item);
  }

  // getProductStock(id: string): Observable<number> {
  //   return this.products$.pipe(
  //     map(products => {
  //       const product = products.find(p => p.id === id)
  //       return product.stock
  //     })
  //   )
  // }


  getProductStocks(id: string) {
    return this.products.find(product => product.id === id).stock;
  }

}
