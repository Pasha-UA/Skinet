import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { IBrand } from 'src/app/shared/models/brand';
import { IProduct, ProductFormValues } from 'src/app/shared/models/product';
import { IType } from 'src/app/shared/models/productType';
import { ShopService } from 'src/app/shop/shop.service';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.scss']
})
export class EditProductComponent implements OnInit {
  productFormValues: ProductFormValues;
  brands: IBrand[];
  types: IType[];
  product: IProduct;

  constructor(private adminService: AdminService,
    private shopService: ShopService,
    private route: ActivatedRoute,
    private router: Router) {
    // this.productFormValues = new ProductFormValues();
  }

  ngOnInit(): void {
    const brands = this.getBrands();
    const types = this.getTypes();

    forkJoin([types, brands]).subscribe({
      next: results => {
        this.types = results[0];
        this.brands = results[1];
      },
      error: e => {
        console.log(e);
      },
      complete: () => {
        if (this.route.snapshot.url[0].path === 'edit') {
          this.loadProduct();
        }
      }
    });
  }


  loadProduct() {
    console.log('loading product...')

    this.shopService.getProduct(this.route.snapshot.paramMap.get('id')).subscribe({
      next: (response: any) => {
        console.log(response);
        const productBrandId = this.brands && this.brands.find(x => x.name === response.productBrand).id;
        const productTypeId = this.types && this.types.find(x => x.name === response.productType).id;
        this.product = response;
        this.productFormValues = { ...response, productBrandId, productTypeId };
      }
    });
  }

  getBrands() {
    return this.shopService.getBrands();
  }

  getTypes() {
    return this.shopService.getTypes();
  }

}
