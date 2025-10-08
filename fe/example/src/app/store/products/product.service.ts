import { ApiService } from '../../core/api-service/api.service';
import { environment } from '../../../evironments/environment';
import { Observable } from 'rxjs';
import { PagingResult } from '../../models/paging-result.model';
import { Injectable } from '@angular/core';
import { Product } from '../../models/products/product.model';
import { ProductCategory } from '../../models/products/product-category.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private productApiUrl = `${environment.apiUrl}/api/products`;

  constructor(private apiService: ApiService) {}

  searchProducts(
    keySearch: string,
    categoryId: number,
    pageNumber: number,
    pageSize: number
  ): Observable<PagingResult<Product>> {
    const params = {
      keySearch: keySearch,
      categoryId: categoryId,
      pageNumber: pageNumber,
      pageSize: pageSize,
    };
    return this.apiService.post<PagingResult<Product>>(this.productApiUrl + '/search', params);
  }

  loadCategories(): Observable<ProductCategory[]> {
    return this.apiService.get<ProductCategory[]>(`${environment.apiUrl}/api/productCategories`);
  }
}
