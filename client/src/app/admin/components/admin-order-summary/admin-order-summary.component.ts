import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { IOrder } from 'src/app/shared/models/order';
import { AdminService } from '../../admin.service';


@Component({
  selector: 'app-admin-order-summary',
  templateUrl: './admin-order-summary.component.html',
  styleUrls: ['./admin-order-summary.component.scss']
})

// компонент используется для вывода заказа на админ-панели с возможностью изменять статус заказа

export class AdminOrderSummaryComponent implements OnInit {
  order$: Observable<IOrder>;
  @Output() changeOrderStatus: EventEmitter<IOrder> = new EventEmitter<IOrder>();

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
//    this.order$ = this.adminService.getOrderList();
  }

}
