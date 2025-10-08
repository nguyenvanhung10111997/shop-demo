import { computed, inject } from '@angular/core';
import { signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { CartItem } from '../../models/carts/cart-item.model';
import {
  updateState,
  withDevtools,
  withLocalStorage,
  withStorageSync,
} from '@angular-architects/ngrx-toolkit';
import { OrderService } from '../orders/order.service';
import { Product } from '../../models/products/product.model';
import { OrderCreateRequest, OrderDetailCreateRequest } from '../../models/orders/order.model';
import { OrderDetail } from '../../models/orders/order-detail.model';

interface CartState {
  cart: CartItem[];
  loading: boolean;
}

const initialState: CartState = {
  cart: [],
  loading: false,
};

export const CartStore = signalStore(
  { providedIn: 'root' },
  withDevtools('CartStore'),
  withStorageSync('cart', withLocalStorage()),
  withState(initialState),
  withComputed(({ cart }) => ({
    cartTotal: computed(() => cart().reduce((sum, item) => sum + item.price * item.quantity, 0)),
    cartItemCount: computed(() => cart().reduce((sum, item) => sum + item.quantity, 0)),
  })),
  withMethods((store, orderService = inject(OrderService)) => ({
    addToCart(product: Product): void {
      const currentCart = store.cart();
      const existingItem = currentCart.find((item) => item.id === product.id);

      if (existingItem) {
        const updatedCart = currentCart.map((item) =>
          item.id === product.id
            ? {
                ...item,
                quantity: item.quantity + 1,
                totalPrice: product.price * (item.quantity + 1),
              }
            : item
        );
        updateState(store, 'addToCart update', { cart: updatedCart });
      } else {
        updateState(store, 'addToCart create', {
          cart: [...currentCart, { ...product, quantity: 1, totalPrice: product.price }],
        });
      }
    },
    removeFromCart(cartItem: CartItem): void {
      const currentCart = store.cart();
      const updatedCart = currentCart.filter((item) => item.id !== cartItem.id);
      updateState(store, 'removeFromCart', { cart: updatedCart });
    },
    createOrder(): void {
      const orderDetails = store.cart().map(
        (item) =>
          ({
            productId: item.id,
            productName: item.productName,
            quantity: item.quantity,
            amount: item.totalPrice,
          } as OrderDetailCreateRequest)
      );

      const orderCreateReq: OrderCreateRequest = {
        description: 'Order from cart',
        orderDetails: orderDetails,
      };

      orderService.createOrder(orderCreateReq).subscribe((isSuccess) => {
        if (isSuccess) {
          updateState(store, 'createOrder', { cart: [] });
          window.alert('Order created successfully!');
        } else {
          window.alert('Order created failed!');
        }
      });
    },
  }))
);
