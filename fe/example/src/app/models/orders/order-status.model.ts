export interface OrderStatus {
  id: number;
  statusCode: string;
  statusName: string;
}

export enum OrderStatusEnum {
  Pending = 1,
  Processing = 2,
  Picked = 3,
  Delivering = 4,
  Completed = 5,
  Cancelled = 6
}