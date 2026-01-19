import { Component, inject, OnInit } from '@angular/core';
import { ItemService } from '../../../services/item.service';
import { ApiResponse, ItemResponse } from '../../../models/item.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item',
  imports: [CommonModule],
  templateUrl: './item.html',
  styleUrl: './item.css',
})
export class Item implements OnInit   {

  itemService = inject(ItemService)

  items : ItemResponse[] = []

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => { 
      this.items = res.data;
    });
  }

}
