import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductCategoriesTreeComponent } from './product-categories-tree/product-categories-tree.component';

const routes: Routes = [
  { path: '', component: ShopComponent },
  { path: 'tree', component: ProductCategoriesTreeComponent, data: { breadcrumb: { alias: 'TreeView' } } },
  { path: ':id', component: ProductDetailsComponent, data: { breadcrumb: { alias: 'productDetails' } } },
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ShopRoutingModule { }
