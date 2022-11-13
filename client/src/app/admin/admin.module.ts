import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminOrdersComponent } from './components/admin-orders/admin-orders.component';
import { AdminOrderSummaryComponent } from './components/admin-order-summary/admin-order-summary.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { EditProductFormComponent } from './components/edit-product-form/edit-product-form.component';
import { EditProductPhotosComponent } from './components/edit-product-photos/edit-product-photos.component';



@NgModule({
  declarations: [
    AdminComponent,
    AdminOrdersComponent,
    AdminOrderSummaryComponent,
    EditProductComponent,
    EditProductFormComponent,
    EditProductPhotosComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
