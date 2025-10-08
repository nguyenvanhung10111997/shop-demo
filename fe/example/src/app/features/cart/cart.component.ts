import { Component, inject, OnInit } from '@angular/core';
import { CartStore } from '../../store/carts/cart.store';
import { CartItemComponent } from '../../shared/components/cart-item/cart-item.component';
import { CartItem } from '../../models/carts/cart-item.model';

@Component({
  selector: 'app-cart',
  imports: [CartItemComponent ],
  templateUrl: './cart.component.html',
  standalone: true
})
export class CartComponent implements OnInit {
  cartStore = inject(CartStore);
  cartItems = this.cartStore.cart;
  
  ngOnInit() {

  }

  removeItem(item: CartItem) {
    this.cartStore.removeFromCart(item);
  }

  createOrder() {
    this.cartStore.createOrder();
  }
}