import { OrderDetail } from "./order-detail.model";

export interface Order {
  id: string;
  orderCode: string;
  orderStatusId: number;
  totalQuantity: number;
  totalAmount: number;
  description: string;
  createdAt: string;
  orderDetails: OrderDetail[];
}

export interface OrderCreateRequest {
  description: string;
  orderDetails: OrderDetailCreateRequest[];
}

export interface OrderDetailCreateRequest {
  productId: string;
  productCode: string;
  productName: string;
  amount: number;
  quantity: number;
}