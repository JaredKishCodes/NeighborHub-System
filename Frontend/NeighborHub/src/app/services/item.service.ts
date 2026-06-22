import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, CreateItemRequest, ItemResponse } from '../models/item.model';
import { env } from '../../environments/environment.production';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  private apiUrl = `${env.apiBaseUrl}/api/Item`;
  private http = inject(HttpClient);

  getItems(): Observable<ApiResponse<ItemResponse[]>> {
    return this.http.get<ApiResponse<ItemResponse[]>>(this.apiUrl);
  }

  createItem(itemData: CreateItemRequest): Observable<ApiResponse<ItemResponse>> {
    return this.http.post<ApiResponse<ItemResponse>>(this.apiUrl, itemData);
  }
}
