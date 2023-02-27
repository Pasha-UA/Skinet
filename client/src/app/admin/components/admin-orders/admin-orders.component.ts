import { Component, OnInit } from '@angular/core';
import { IOrderStatus } from 'src/app/shared/models/orderStatus';
import { IOrder } from 'src/app/shared/models/order';
import { AdminService } from '../../admin.service';
import { tap, map, Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {
  orders: IOrder[];
  orderStatusList = [];
  orders$: Observable<IOrder>;

  constructor(private adminService: AdminService, private toastr: ToastrService,) { }

  ngOnInit(): void {
    this.getOrders();
    this.getOrderStatusList();
  }

  getOrders() {
    this.orders$ = this.adminService.getOrderList() as Observable<IOrder>;
    this.adminService.getOrderList()
      .subscribe({
        next: (orders: IOrder[]) => {
          // console.log(orders);
          this.orders = orders;
        },
        error: (e) => console.error(e)
      });
  }

  getOrderStatusList() {
    this.adminService.getOrderStatusList()
      .subscribe({
        next: (response: IOrderStatus[]) => {
          this.orderStatusList = response;
          // console.log(this.orderStatusList)
        },
        error: (e) => {
          console.log(e);
        }
      });

  }

  onOrderStatusChanged(orderId: string, orderStatusValue: string) {
    // if selectedOrder.status !== orderStatus {save new status to db}

    orderStatusValue = orderStatusValue.split(/[: ]+/).pop();
    let orderStatusId = this.orderStatusList.find(os => os.value === orderStatusValue).id;
    this.adminService.updateOrderStatus(orderId, orderStatusId)
      .subscribe({
        next: (response: IOrder) => {
          // const status = this.orderStatusList.find(st => { st.id === response.status })
          // console.log(response)
          this.toastr.success(`Order # ${orderId} status modified. New status is ${this.findStatusNameById(response.statusId)}`);

        },
        error: error => {
          console.log(error)
          this.toastr.error('не удалось обновить статус заказа');
        }
      })

  }

  findStatusNameById(statusId: string): string {
//    let status = this.orderStatusList.find(st => { return st.id === statusId })
    return this.orderStatusList.find(st => { return st.id === statusId }).name;
  }

  findStatusNameByValue(statusValue: string): string {
    // console.log(statusValue);
    // let status = this.orderStatusList.find(st => { return st.value === statusValue })
    // console.log(status);
    return this.orderStatusList.find(st => { return st.value === statusValue }).name;
  }
}
