import { Component, input, output } from '@angular/core';
import { CartItem } from '../../../models/carts/cart-item.model';
import { Order } from '../../../models/orders/order.model';
import { DatePipe } from '@angular/common';
import { OrderStatus, OrderStatusEnum } from '../../../models/orders/order-status.model';
import { OrderDetailInfoComponent } from '../order-detail-info/order-detail-info.component';

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  standalone: true,
  imports: [DatePipe, OrderDetailInfoComponent],
})
export class OrderItemComponent {
  order = input.required<Order>();
  orderStatuses = input.required<OrderStatus[]>();
  toggleExpansion = output<string>();

  getStatusClass(status: OrderStatusEnum): string {
    const classes: Record<OrderStatusEnum, string> = {
      [OrderStatusEnum.Pending]: 'px-3 py-1 rounded-full text-amber-600 bg-amber-50',
      [OrderStatusEnum.Processing]: 'px-3 py-1 rounded-full text-blue-600 bg-blue-50',
      [OrderStatusEnum.Picked]: 'px-3 py-1 rounded-full text-purple-600 bg-purple-50',
      [OrderStatusEnum.Delivering]: 'px-3 py-1 rounded-full text-yellow-600 bg-yellow-50',
      [OrderStatusEnum.Completed]: 'px-3 py-1 rounded-full text-green-600 bg-green-50',
      [OrderStatusEnum.Cancelled]: 'px-3 py-1 rounded-full text-red-600 bg-red-50',
    };

    return classes[status];
  }

  getStatusLabel(status: OrderStatusEnum): string {
    return this.orderStatuses().find((s) => s.id === status)?.statusName || 'Unknown';
  }
}
