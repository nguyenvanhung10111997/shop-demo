import { Component, input, output } from '@angular/core';
import { CartItem } from '../../../models/carts/cart-item.model';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  standalone: true
})
export class CartItemComponent {
  cartItem = input.required<CartItem>();
  removeItem = output<CartItem>();
}