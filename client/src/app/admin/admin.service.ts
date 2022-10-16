import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrderList() {
    return this.http.get(this.baseUrl + 'admin/orders');
  }

  updateOrderStatus(orderId: string, orderStatus: string) {
    console.log(orderId, ' ', orderStatus);
    return this.http.put(this.baseUrl + 'admin/order/' + orderId, orderStatus);
  }

  // getOrderStatuses() {
  //   return this.http.get(this.baseUrl + 'admin/orderstatuses');
  // }

}
