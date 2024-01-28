import { IProductPrice } from "./productPrice";

export interface IProduct {
    id: string;
    name: string;
    description: string;
    price: number;
    pictureUrl: string;
    productType: string;
    productBrand: string;
    productCategory: string;
    stock: number;
    barCode: string;
    photos: IPhoto[];
    prices: IProductPrice[];
    visible: boolean;
}

export interface IPhoto {
  id: number;
  pictureUrl: string;
  fileName: string;
  isMain: boolean;
}

export interface IProductToCreate {
    name: string;
    description: string;
    price: number;
    pictureUrl: string;
    productTypeId: number;
    productBrandId: number;
  }
  
  export class ProductFormValues implements IProductToCreate {
    name = '';
    description = '';
    price = 0;
    pictureUrl = '';
    productBrandId: number;
    productTypeId: number;
  
    constructor(init?: ProductFormValues) {
      Object.assign(this, init);
    }
  }
  