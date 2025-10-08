import { Component, effect, inject, OnInit } from '@angular/core';
import { OrderStore } from '../../store/orders/order.store';
import { OrderStatus, OrderStatusEnum } from '../../models/orders/order-status.model';
import { DatePipe } from '@angular/common';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';


@Component({
  selector: 'app-order',
  imports: [DatePipe],
  templateUrl: './order.component.html',
  standalone: true,
})
export class OrderComponent implements OnInit {
  orderStore = inject(OrderStore);
  filteredOrders = this.orderStore.orders;
  private initialFilter = { id: -1, statusCode: 'all', statusName: 'All Orders' };
  filters: OrderStatus[] = [this.initialFilter];

  constructor() {
    this.initialLoad();
  }

  ngOnInit() {
    this.orderStore.loadOrderStatuses();
  }

  initialLoad = () => {
    const orderStatuses$ = toObservable(this.orderStore.orderStatuses);

    orderStatuses$.pipe(takeUntilDestroyed()).subscribe((statuses) => {
      if (statuses?.length) {
        this.filters = [this.initialFilter, ...statuses];
        this.setFilter(-1);
      }
    });
  };

  setFilter(status: number | '-1') {
    this.orderStore.setStatusFilter(status);
    this.orderStore.loadOrders();
  }

  toggleExpansion(orderId: string) {
    this.orderStore.toggleOrderExpansion(orderId);
    this.orderStore.loadOrderDetails(orderId);
  }

  isExpanded(orderId: string): boolean {
    return this.orderStore.expandedOrderId() === orderId;
  }

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
    const orderStatuses = this.orderStore.orderStatuses();
    return orderStatuses.find((s) => s.id === status)?.statusName || 'Unknown';
  }
}
