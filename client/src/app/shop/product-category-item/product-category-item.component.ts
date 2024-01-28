import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICategory } from 'src/app/shared/models/productCategory';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-category-item',
  templateUrl: './product-category-item.component.html',
  styleUrls: ['./product-category-item.component.scss']
})
export class ProductCategoryItemComponent implements OnInit {
  @Output() categorySelected: EventEmitter<string> = new EventEmitter<string>();

  @Input() category: ICategory;
  // childProductCount: number;
  childCategoryCount: number;
  totalChildProductsCount: number;

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    // this.shopService.getDirectChildrenProductsCount(this.category.id).subscribe({
    //   next: response => this.childProductCount = response
    // });

    this.childCategoryCount = this.shopService.getChildrenCategoriesCount(this.category.id);

    this.shopService.getChildrenProductsCount(this.category.id).subscribe({
      next: response => this.totalChildProductsCount = response
    })

  }

  categoryClicked(category) {
    this.categorySelected.emit(category.id);

  }

}
