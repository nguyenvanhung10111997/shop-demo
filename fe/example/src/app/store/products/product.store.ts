import { computed, inject } from '@angular/core';
import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { Product } from '../../models/products/product.model';
import { CartItem } from '../../models/carts/cart-item.model';
import { updateState, withDevtools } from '@angular-architects/ngrx-toolkit';
import { ProductCategory } from '../../models/products/product-category.model';
import { ProductService } from './product.service';

interface ProductState {
  products: Product[];
  loading: boolean;
  keySearch: string;
  selectedCategory: number | '-1';
  categories: ProductCategory[];
  totalRecords: number;
}

const initialState: ProductState = {
  products: [],
  loading: false,
  keySearch: '',
  selectedCategory: '-1',
  categories: [],
  totalRecords: 0
};

export const ProductStore = signalStore(
  { providedIn: 'root' },
  withDevtools('ProductStore'),
  withState(initialState),
  withMethods((store, productService = inject(ProductService)) => ({
    loadProducts(): void {
      updateState(store, 'loadProducts start', { loading: true });

      const selectedCategoryId = store.selectedCategory() as number;
      productService.searchProducts(store.keySearch(), selectedCategoryId, 0, 100).subscribe(x =>{
        updateState(store, 'loadProducts end', { products: x.records, totalRecords: x.totalRecords, loading: false });
      });
    },

    loadCategories(): void {
      productService.loadCategories().subscribe((categories) => {
        updateState(store, 'loadCategories', { categories: categories });
      });
    },

    setSearchTerm(term: string): void {
      updateState(store, 'setSearchTerm', { keySearch: term });
    },

    setSelectedCategory(categoryId: number): void {
      updateState(store, 'setSelectedCategory', { selectedCategory: categoryId });
    }
  }))
);
