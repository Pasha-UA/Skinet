import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { ICategory, ICategoryTree } from '../shared/models/productCategory';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';
import { CdkTreeModule } from '@angular/cdk/tree';
import { NestedTreeControl } from '@angular/cdk/tree';


@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  @Input() categoryId: string;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  selectedCategory: ICategory;
  childrenCategories: ICategory[]
  categoriesTree: ICategoryTree[];
  shopParams = new ShopParams;
  totalCount: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ];


  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
    // this.getCategories();
    this.getCategoriesTree();
  }


  getProducts(useCache = false) {
    this.shopService.getProducts(useCache).subscribe(
      {
        next: response => {
          this.products = response.data;
          this.totalCount = response.count;
          console.log(this.products);
        },
        error: error => {
          console.log(error);
        }
      }
    );
  }

  getBrands() {
    this.shopService.getBrands().subscribe(
      {
        next: response => {
          this.brands = [{ id: 0, name: 'All' }, ...response];
        }
        ,
        error: error => {
          console.log(error);
        }
      }
    );
  }


  getTypes() {
    this.shopService.getTypes().subscribe(
      {
        next: response => {
          this.types = [{ id: 0, name: 'All' }, ...response];
        },
        error: error => {
          console.log(error);
        }
      }
    );
  }

  // getCategories() {
  //   this.shopService.getCategories().subscribe(
  //     {
  //       next: response => {
  //         //          this.categories = [{ id: 0, name: 'All' }, ...response];
  //       },
  //       error: error => {
  //         console.log(error);
  //       }
  //     }
  //   );
  // }

  getCategoriesTree() {
    this.shopService.getCategoriesTree().subscribe({
      next: (response) => {
        this.categoriesTree = response;
        //        console.log(this.categoriesTree);
      }
    })
  }

  findChildren(tree: ICategoryTree[], id: string): ICategoryTree[] {
    return this.shopService.getChildrenCategories(this.categoriesTree, id);
  }

  onBrandSelected(brandId: number) {
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number) {
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onSortSelected(sort: string) {
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onPageChanged(event: any) {
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }
  }

  onSearch() {
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }

  onCategorySelect(event: any) {
    const params = this.shopService.getShopParams();

    //console.log(event);
    params.categoryId = event;

    this.shopService.setShopParams(params);

    this.childrenCategories = this.findChildren(this.categoriesTree, event);

    this.getProducts();
  }

}
