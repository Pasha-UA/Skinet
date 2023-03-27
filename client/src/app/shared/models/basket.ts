import { v4 as uuidv4 } from 'uuid'
import { IProductPrice } from './productPrice';


export interface IBasketItem {
    id: string;
    name: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
    prices: IProductPrice[];
}

export interface IBasket {
    id: string;
    items: IBasketItem[];
    deliveryMethodId?: string;
}

export class Basket implements IBasket {
    id = uuidv4();
    items: IBasketItem[] = [];
}

export interface IBasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}
