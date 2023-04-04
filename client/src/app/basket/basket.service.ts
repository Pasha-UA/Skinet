import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IProduct } from '../shared/models/product';
import { IProductPrice } from '../shared/models/productPrice';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  shipping = 0;

  constructor(private httpClient: HttpClient) { }

  getBasket(id: string) {
    return this.httpClient.get(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        }
        ));
  }

  setShippingPrice(deliveryMethod: IDeliveryMethod) {
    this.shipping = deliveryMethod.price;
    const basket = this.getCurrentBasketValue();
    basket.deliveryMethodId = deliveryMethod.id;
    this.calculateTotals();
    this.setBasket(basket);

  }

  setBasket(basket: IBasket) {
    return this.httpClient.post(this.baseUrl + 'basket', basket).subscribe(
      {
        next: (response: IBasket) => {
          this.basketSource.next(response);
          this.calculateTotals();
          console.log(response);
        },
        error: error => {
          console.log(error);
        }
      })
  }


  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);

    basket.items = this.addOrUpdateItem(basket.items, basket.items[foundItemIndex], 1);

    // basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items = this.addOrUpdateItem(basket.items, basket.items[foundItemIndex], -1);

      // basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }

  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket.items.some(x => x.id === item.id)) {
      basket.items = basket.items.filter(i => i.id !== item.id);
    }
    if (basket.items.length > 0) {
      this.setBasket(basket);
    } else {
      this.deleteBasket(basket);
    }
  }

  deleteLocalBasket(id: string) {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  deleteBasket(basket: IBasket) {
    return this.httpClient.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(
      {
        next: () => {
          this.basketSource.next(null);
          this.basketTotalSource.next(null);
          localStorage.removeItem('basket_id');
        },
        error: error => {
          console.log(error);
        }
      });
  }


  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    else {
      items[index].quantity += quantity;

      // recalculate price 
      const recalculatedPrice = this.calculateItemPrice(items[index], items[index].quantity).value; 
      items[index].price = recalculatedPrice;
    }

    return items;

  }

  private createBasket(): IBasket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    const basketItem: IBasketItem = {
      id: item.id,
      name: item.name,
      brand: item.productBrand,
      type: item.productType,
      // price: item.price,
      // price: price,
      price: this.calculateItemPrice(item, quantity).value,
      pictureUrl: item.pictureUrl,
      quantity,
      prices: item.prices
    }

    return basketItem;
  }

  // calculates price for item dependent of quantity ordered
  private calculateItemPrice(item: IProduct | IBasketItem, quantity: number): IProductPrice {
    let price: IProductPrice;
    if (item.prices && item.prices.length > 0) {
      const prices = item.prices;
      const filteredPricesArr: IProductPrice[] = prices.filter((price) => price.priceType.quantity <= quantity);
      const maxQuantity: number = Math.max(...filteredPricesArr.map((price) => price.priceType.quantity));
      price = filteredPricesArr.find((price) => price.priceType.quantity === maxQuantity);
      // const price: number = filteredPricesArr.find((price) => price.priceType.quantity === maxQuantity).value;
    }
    else {
      price = item.prices.find(x=>x.priceType.quantity==1);
    }

    return price;
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0) // суммирование произведений количества на цену по всей корзине
    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }

}
