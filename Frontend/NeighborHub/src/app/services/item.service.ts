import { inject, Injectable } from '@angular/core';
import { env } from '../../environments/environment.production';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse, ItemResponse } from '../models/item.model';

@Injectable({
  providedIn: 'root',
})
export class ItemService {

  private apiUrl = env.apiBaseUrl + '/api/Item';

  private http  = inject(HttpClient);

  getItems() :Observable<ApiResponse<ItemResponse[]>>
   { 
    return this.http.get<ApiResponse<ItemResponse[]>>(this.apiUrl);
   }

   createItem(itemData: any, imageFile: File): Observable<ApiResponse<ItemResponse>> {
    const formData = new FormData();
    formData.append('name', itemData.name);
    formData.append('description', itemData.description);
    formData.append('category', itemData.category);
    formData.append('itemStatus', itemData.itemStatus.toString());
    formData.append('createdAt', itemData.createdAt.toISOString());
    formData.append('ownerId', itemData.ownerId.toString());

    if (imageFile) {
      formData.append('imageUrl', imageFile);
    }
    return this.http.post<ApiResponse<ItemResponse>>(this.apiUrl, formData);}
}
