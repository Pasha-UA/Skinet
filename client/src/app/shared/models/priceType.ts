import { ICurrency } from "./currency";

export interface IPriceType {
    quantity: number;
    currencyId: string;
    currency: ICurrency;
    isRetail: boolean;
    isBulk: boolean;
    id: string;
};