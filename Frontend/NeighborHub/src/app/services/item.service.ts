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
}
