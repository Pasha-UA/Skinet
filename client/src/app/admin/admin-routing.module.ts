import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { AdminOrdersComponent } from './components/admin-orders/admin-orders.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';

const routes: Routes = [
  { path: '', canActivate: [AuthGuard /*, RoleGuard*/], component: AdminComponent/*, data: {role: 'administrator'} */ },
  { path: 'create', component: EditProductComponent, data: { breadcrumb: 'Create' } },
  { path: 'edit/:id', component: EditProductComponent, data: { breadcrumb: 'Edit' } },
  { path: 'orders', canActivate: [AuthGuard, /* RoleGuard */], component: AdminOrdersComponent/*, data: {role: 'Admin'} */ }
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
export class AdminRoutingModule { }
