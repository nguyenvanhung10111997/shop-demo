import { Product } from "../products/product.model";

export interface CartItem extends Product {
  quantity: number;
  totalPrice: number;
}