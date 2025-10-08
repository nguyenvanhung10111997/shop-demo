import { Component, inject, input, OnInit, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';
import { ProductStore } from '../../store/products/product.store';
import { CartStore } from '../../store/carts/cart.store';
import { Product } from '../../models/products/product.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  standalone: true,
  imports: [ProductCardComponent, ScrollingModule, FormsModule ]
})
export class ProductComponent implements OnInit {
  productStore = inject(ProductStore);
  cartStore = inject(CartStore);
  
  filteredProducts = this.productStore.products;
  categories = this.productStore.categories;

  ngOnInit() {
    this.productStore.loadCategories();
    this.selectCategory(-1);
  }

  onSearchChange(term: string) {
    this.productStore.setSearchTerm(term);
    this.productStore.loadProducts();
  }

  selectCategory(categoryId: number) {
    this.productStore.setSelectedCategory(categoryId);
    this.productStore.loadProducts();
  }

  onAddToCart(product: Product) {
    this.cartStore.addToCart(product);
    console.log(`Added ${product.productName} to cart`);
  }
}