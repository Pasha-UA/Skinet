import { Component, OnInit } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from '../shop/shop.service';
import { AdminService } from './admin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  products: IProduct[];
  totalCount: number;
  shopParams: ShopParams;

  constructor(private shopService: ShopService, private adminService: AdminService) {
    this.shopParams = this.shopService.getShopParams();
    this.shopParams.search = '';
//    this.shopParams.pageSize = 20;
  }
  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(useCache = false) {
    this.shopService.getProducts(useCache).subscribe({
      next: response => {
        this.products = response.data;
        this.totalCount = response.count;
      },
      error: error => {
        console.log(error);
      }
    });
  }

  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();
    console.log(params);
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }
  }

  deleteProduct(id: string) {
    this.adminService.deleteProduct(id).subscribe({
      next: (response: any) => {
        this.products.splice(this.products.findIndex(p => p.id === id), 1);
        this.totalCount--;
      }
    });
  }

}
