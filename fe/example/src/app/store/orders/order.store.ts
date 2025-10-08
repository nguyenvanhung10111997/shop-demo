import { computed, inject } from '@angular/core';
import { patchState, signalStore, withMethods, withState } from '@ngrx/signals';
import { Order } from '../../models/orders/order.model';
import { OrderStatus } from '../../models/orders/order-status.model';
import { OrderService } from './order.service';
import { map } from 'rxjs';
import { updateState, withDevtools } from '@angular-architects/ngrx-toolkit';
import { create } from 'domain';

interface OrderState {
  orders: Order[];
  loading: boolean;
  expandedOrderId: string | null;
  statusFilter: number | '-1';
  orderStatuses: OrderStatus[];
  totalRecords: number;
}

const initialState: OrderState = {
  orders: [],
  loading: false,
  expandedOrderId: null,
  statusFilter: '-1',
  orderStatuses: [],
  totalRecords: 0
};

export const OrderStore = signalStore(
  { providedIn: 'root' },
  withDevtools('OrderStore'),
  withState(initialState),
  withMethods((store, orderService = inject(OrderService)) => ({
    loadOrders(): void {
      updateState(store, 'loadOrders start', { loading: true });

      const statusFilter = store.statusFilter() as number;
      orderService.searchOrders(statusFilter, 0, 15).subscribe(x =>{
        updateState(store, 'loadOrders end', { orders: x.records, totalRecords: x.totalRecords, loading: false });
      });
    },
    loadOrderDetails(orderId: string): void {
      orderService.getOrderDetails(orderId).pipe(
        map(orderDetails => {
          const order = store.orders().find(o => o.id === orderId);
          if (order) {
            order.orderDetails = orderDetails;
          }
          return order;
        })
      ).subscribe(() =>{
        updateState(store, 'loadOrderDetails', { orders: [...store.orders()] });
      });
    },
    loadOrderStatuses(): void {
      orderService.loadOrderStatuses().subscribe((statuses) => {
        updateState(store, 'loadOrderStatuses', { orderStatuses: statuses });
      });
    },
    setStatusFilter(status: number | '-1'): void {
      updateState(store, 'setStatusFilter', { statusFilter: status });
    },

    toggleOrderExpansion(orderId: string): void {
      const currentId = store.expandedOrderId();
      updateState(store, 'toggleOrderExpansion', {  expandedOrderId: currentId === orderId ? null : orderId });
    }
  }))
);
