import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CartStore } from '../../../store/carts/cart.store';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  standalone: true
})
export class HeaderComponent {
  cartStore = inject(CartStore);
  private router = inject(Router);
  
  cartItemCount = this.cartStore.cartItemCount;

  navigateToProducts = () => {
    this.router.navigate(['products']);
  }

  navigateToOrders = () => {
    this.router.navigate(['orders']);
  }

  navigateToCart = () => {
    this.router.navigate(['cart']);
  }
}