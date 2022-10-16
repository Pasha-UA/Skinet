import { Component, OnInit } from '@angular/core';
import { IOrderStatus } from 'src/app/shared/models/orderStatus';
import { IOrder } from 'src/app/shared/models/order';
import { AdminService } from '../../admin.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.scss']
})
export class AdminOrdersComponent implements OnInit {
  orders: IOrder[];
  orderStatuses = [];
  orders$: Observable<IOrder>;

  constructor(private adminService: AdminService) { }

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
    // HARDCODED STATUSES
    this.orderStatuses = [
      { id: 0, value: 'Pending', name: 'Pending' },
      { id: 1, value: 'PaymentReceived', name: 'Payment Received' },
      { id: 2, value: 'PaymentFailed', name: 'Payment Failed' }
    ];


    // TODO: need load statuses from backend. 

    // this.adminService.getOrderStatuses().subscribe({
    //   next: (response: IOrderStatus) => {
    //     this.orderStatuses = [response];
    //     console.log(response);
    //   },
    //   error: (e) => {
    //     console.log(e);
    //   }
    // });

  }

  onOrderStatusChanged(orderId: string, orderStatus: string) {
    // if selectedOrder.status !== orderStatus {save new status to db}

    this.adminService.updateOrderStatus(orderId, orderStatus)


    // console.log(orderStatus);
  }

}
