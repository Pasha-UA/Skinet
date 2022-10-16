import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { SharedModule } from '../shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminOrdersComponent } from './components/admin-orders/admin-orders.component';
import { FormsModule } from '@angular/forms';
import { AdminOrderSummaryComponent } from './components/admin-order-summary/admin-order-summary.component';



@NgModule({
  declarations: [
    AdminComponent,
    AdminOrdersComponent,
    AdminOrderSummaryComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
