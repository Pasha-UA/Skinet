import { Component, OnInit } from '@angular/core';
import { IOrderStatus } from 'src/app/shared/models/orderStatus';
import { IOrder } from 'src/app/shared/models/order';
import { AdminService } from '../../admin.service';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {
  orders: IOrder[];
  orderStatuses = [];
  orders$: Observable<IOrder>;

  constructor(private adminService: AdminService, private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.getOrders();
    this.getOrderStatuses();
  }

  getOrders() {
    this.orders$ = this.adminService.getOrderList() as Observable<IOrder>;
    this.adminService.getOrderList().subscribe({
      next: (orders: IOrder[]) => {
        this.orders = orders;
      },
      error: (e) => console.error(e)
    });
  }

  getOrderStatuses() {
    this.adminService.getOrderStatuses().subscribe({
      next: (response: IOrderStatus[]) => {
        this.orderStatuses = Object.values(response);
        // console.log(this.orderStatuses)
      },
      error: (e) => {
        console.log(e);
      }
    });

  }

  onOrderStatusChanged(orderId: string, orderStatus: string) {
    // if selectedOrder.status !== orderStatus {save new status to db}

    this.adminService.updateOrderStatus(orderId, orderStatus)
      .subscribe({
        next: (response: IOrder) => {
          const status = this.orderStatuses.find(st => { return st.id === response.status })
          this.toastr.success(`Order # ${orderId} status modified. New status is ${this.findStatusNameById(response.status)}`);

        },
        error: error => {
          console.log(error)
          this.toastr.error('не удалось обновить статус заказа');
        }
      })

  }

  findStatusNameById(statusId: string): string {
    return this.orderStatuses.find(st => { return st.id === statusId }).name;
  }

}
