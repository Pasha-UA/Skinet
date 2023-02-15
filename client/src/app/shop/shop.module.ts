import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';
import { MatTreeModule } from '@angular/material/tree';
import { ProductCategoriesTreeComponent } from './product-categories-tree/product-categories-tree.component';
import { ProductCategoryItemComponent } from './product-category-item/product-category-item.component';



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductCategoriesTreeComponent,
    ProductCategoryItemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ShopRoutingModule,
    MatTreeModule
  ],
  exports: []
})
export class ShopModule { }
