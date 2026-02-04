import { Component, CUSTOM_ELEMENTS_SCHEMA, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { ItemService } from '../../../services/item.service';
import { ApiResponse, ItemResponse } from '../../../models/item.model';
import { CommonModule } from '@angular/common';
import { env } from '../../../../environments/environment.production';

import { FormsModule } from '@angular/forms';
import { BookingComponent } from "../booking/booking";

@Component({
  selector: 'app-item',
  imports: [CommonModule, FormsModule, BookingComponent],
  templateUrl: './item.html',
  styleUrl: './item.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class Item implements OnInit   {

  itemService = inject(ItemService)
   startDate: string | null = null;
  endDate: string | null = null;

  
  readonly baseUrl = env.apiBaseUrl;

  items : ItemResponse[] = []

  

    

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => { 
      this.items = res.data;
    });
  }

  

}
