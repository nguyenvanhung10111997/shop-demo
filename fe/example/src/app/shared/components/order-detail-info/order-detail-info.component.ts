import { Component, input, output } from '@angular/core';
import { Order } from '../../../models/orders/order.model';

@Component({
  selector: 'app-order-detail-info',
  templateUrl: './order-detail-info.component.html',
  standalone: true
})
export class OrderDetailInfoComponent {
  order = input.required<Order>();
}
