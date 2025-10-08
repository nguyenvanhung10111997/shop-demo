import { ApiService } from '../../core/api-service/api.service';
import { environment } from '../../../evironments/environment';
import { Observable } from 'rxjs';
import { PagingResult } from '../../models/paging-result.model';
import { Order, OrderCreateRequest } from '../../models/orders/order.model';
import { Injectable } from '@angular/core';
import { OrderStatus } from '../../models/orders/order-status.model';
import { OrderDetail } from '../../models/orders/order-detail.model';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private orderApiUrl = `${environment.apiUrl}/api/orders`;

  constructor(private apiService: ApiService) {}

  searchOrders(
    statusId: number,
    pageNumber: number,
    pageSize: number
  ): Observable<PagingResult<Order>> {
    const params = {
      orderStatusId: statusId,
      pageNumber: pageNumber,
      pageSize: pageSize,
    };
    return this.apiService.post<PagingResult<Order>>(this.orderApiUrl + '/search', params);
  }

  createOrder(order: OrderCreateRequest): Observable<boolean> {
    return this.apiService.post<boolean>(this.orderApiUrl + '/create', order);
  }

  getOrderDetails(orderId: string): Observable<OrderDetail[]> {
    return this.apiService.get<OrderDetail[]>(`${this.orderApiUrl}/${orderId}/details`);
  }

  loadOrderStatuses(): Observable<OrderStatus[]> {
    return this.apiService.get<OrderStatus[]>(`${environment.apiUrl}/api/orderStatus`);
  }
}
