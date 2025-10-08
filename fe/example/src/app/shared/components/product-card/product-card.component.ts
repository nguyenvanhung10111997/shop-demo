import { Component, input, output } from '@angular/core';
import { Product } from '../../../models/products/product.model';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  standalone: true
})
export class ProductCardComponent {
  product = input.required<Product>();
  addToCart = output<Product>();

  getStars(rating: number): string {
    const full = Math.floor(rating);
    const empty = 5 - full;
    return '★'.repeat(full) + '☆'.repeat(empty);
  }
}