import { Component, inject, OnInit } from '@angular/core';
import { OrderStore } from '../../store/orders/order.store';
import { OrderStatus } from '../../models/orders/order-status.model';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { OrderItemComponent } from '../../shared/components/order-item/order-item.component';


@Component({
  selector: 'app-order',
  imports: [OrderItemComponent],
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
}
