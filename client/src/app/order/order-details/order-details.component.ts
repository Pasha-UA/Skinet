import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IOrder } from 'src/app/shared/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: IOrder;
  constructor(private route: ActivatedRoute, private breadcrumbService: BreadcrumbService, private ordersService: OrdersService) {
    this.breadcrumbService.set('@OrderDetails', '');
  }

  ngOnInit(): void {
    this.ordersService.getOrderDetails(+this.route.snapshot.paramMap.get('id')).subscribe({
      next: (order: IOrder) => {
        this.order = order,
          this.breadcrumbService.set('@OrderDetails', `Order # ${order.id} - ${order.status}`);
      },
      error: (e) => console.error(e)
    })
  }
}
