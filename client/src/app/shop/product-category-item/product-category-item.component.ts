import { Component, Input, OnInit } from '@angular/core';
import { ICategory } from 'src/app/shared/models/productCategory';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-category-item',
  templateUrl: './product-category-item.component.html',
  styleUrls: ['./product-category-item.component.scss']
})
export class ProductCategoryItemComponent implements OnInit {
  @Input() category: ICategory;
  childProductCount: number;
  childCategoryCount: number;

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.shopService.getChildrenProductsCount(this.category.id).subscribe({
      next: response => this.childProductCount=response
    });

    this.childCategoryCount = this.shopService.getChildrenCategoriesCount(this.category.id);
  }

}
