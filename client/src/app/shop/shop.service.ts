import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { IBrand } from '../shared/models/brand';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { ICategory, ICategoryTree } from '../shared/models/productCategory';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/';
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  categories: ICategory[] = [];
  categoriesTree: ICategoryTree[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.products = [];
    }
    console.log(this.shopParams);

    if (this.products.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.products.length / this.shopParams.pageSize);

      if (this.shopParams.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.products.slice((this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
            this.shopParams.pageNumber * this.shopParams.pageSize);
            console.log(this.products);
        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandId !== 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    if (this.shopParams.categoryId) {
      params = params.append('categoryId', this.shopParams.categoryId);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          this.products = [...this.products, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getShopParams() {
    return this.shopParams;
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }


  getProduct(id: string) {
    const product = this.products.find(p => p.id === id);

    if (product) {
      return of(product);
    }

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  getTypes() {
    if (this.types.length > 0) {
      return of(this.types);
    }
    return this.http.get<IType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );
  }

  getCategories() {
    if (this.categories.length > 0) {
      return of(this.categories)
    }
    return this.http.get<ICategory[]>(this.baseUrl + 'products/categories').pipe(
      map(response => {
        this.categories = response;
        return response;
      })
    );
  }

  getCategoriesTree() {
    return this.getCategories().pipe(
      map(categories => {
        const categoriesMap = new Map(categories.map(c => [c.id, c]));
        this.categoriesTree = this.buildTree(categoriesMap, null, 0);
        return this.categoriesTree;
      })
    )
  }

  private buildTree(categoriesMap, parentId, level) {
    const children: ICategoryTree[] = [];
    for (const [id, category] of categoriesMap) {
      if (category.parentId === parentId) {
        children.push({
          ...category,
          level,
          children: this.buildTree(categoriesMap, id, level + 1)
        });
      }
    }
    return children;
  }

  getChildrenCategories(tree: ICategoryTree[], categoryId: string): ICategoryTree[] {
    for (const node of tree) {
      if (node.id === categoryId) {
        return node.children;
      }
      const children = this.getChildrenCategories(node.children, categoryId);
      if (children) {
        return children;
      }
    }
    return undefined;
  }

  getChildrenProductsCount(categoryId: string) {

    let params = this.getShopParams();
    params.categoryId = categoryId;
    let productsCount = this.getProducts(true).pipe(
      map(response => {
        return response.count
      })
    );

    return productsCount;
  }

  getChildrenCategoriesCount(categoryId: string) {
    if (!Array.isArray(this.categories) || !this.categories.length) this.getCategories();
  
    return this.categories.filter(c=> c.parentId===categoryId).length
  }

  getPriceTypes () {
    
  }

}
