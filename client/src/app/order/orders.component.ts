import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin/admin.service';
import { IOrder } from '../shared/models/order';
import { OrdersService } from './orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: IOrder[];

  constructor(private ordersService: OrdersService, private adminService: AdminService) { }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    this.ordersService.getOrdersForUser().subscribe({
      next: (orders: IOrder[]) => {
        this.orders = orders;
        console.log(this.orders);
        console.log('orders');  
      },
      error: (e)=>console.error(e)
    });

    // this.ordersService.getOrdersForUser().subscribe((orders: IOrder[])=>{
    //   this.orders = orders;
    //   console.log(this.orders);
    //   console.log('orders');
    // }, error=>{
    //   console.log(error);
    // })
  }

}
