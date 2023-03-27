import { IPriceType } from "./priceType";

export interface IProductPrice {
    value: number;
    productId: string;
    priceTypeId: string;
    priceType: IPriceType;
    dateTime: string;
    id: string;
}