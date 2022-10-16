import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { AdminOrdersComponent } from './components/admin-orders/admin-orders.component';

const routes: Routes = [
  { path: '', canActivate: [AuthGuard /*, RoleGuard*/], component: AdminComponent/*, data: {role: 'administrator'} */},
  { path: 'orders', canActivate: [AuthGuard , /* RoleGuard */ ], component: AdminOrdersComponent/*, data: {role: 'Admin'} */}
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
