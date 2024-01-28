import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';
import { IProductPrice } from 'src/app/shared/models/productPrice';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit {

  @Input() product: IProduct;

  productPrices: IProductPrice[];
  retailPrice: IProductPrice;
  bulkPrice: IProductPrice;
  canAddToBasket: boolean;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.productPrices = this.product.prices
        // .filter(p => (p.priceType.isRetail || p.priceType.isBulk) == false)
        .filter(p => (p.priceType.isBulk) == false)
        .sort((a, b)=>(a.priceType.quantity-b.priceType.quantity));
    this.retailPrice = this.product.prices.find(p => p.priceType.isRetail == true);
    this.bulkPrice = this.product.prices.find(p => p.priceType.isBulk == true);
    this.canAddToBasket = this.product.visible && (this.product.stock > 0)
  }

  addItemToBasket() {
    console.log(this.product);
    this.basketService.addItemToBasket(this.product);
  }

}
