import { Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnInit } from '@angular/core';
import { ItemService } from '../../../services/item.service';
import { ApiResponse, ItemResponse } from '../../../models/item.model';
import { CommonModule } from '@angular/common';
import { env } from '../../../../environments/environment.production';
import { DialogRef } from '@angular/cdk/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-item',
  imports: [CommonModule,FormsModule],
  templateUrl: './item.html',
  styleUrl: './item.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class Item implements OnInit   {

  itemService = inject(ItemService)
   startDate: string | null = null;
  endDate: string | null = null;

  private dialogRef = inject(DialogRef);
  
  readonly baseUrl = env.apiBaseUrl;

  items : ItemResponse[] = []


    updateStartDate(event: Event) {
    const target = event.target as any;  // Safe cast to access 'value' on custom element
    this.startDate = target?.value || '';
  }

  updateEndDate(event: Event) {
    const target = event.target as any;  // Safe cast to access 'value' on custom element
    this.endDate = target?.value || '';
  }

  closeModal() {
    this.dialogRef.close();
  }

  handleBooking() {
    if (!this.startDate || !this.endDate) {
      alert('Please select both start and end dates.');
      return;
    }
    if (new Date(this.startDate) >= new Date(this.endDate)) {
      alert('End date must be after start date.');
      return;
    }
    alert(`Booking confirmed from ${this.startDate} to ${this.endDate}!`);
    // Replace with your booking logic (e.g., call a service)
    this.closeModal();
  }

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => { 
      this.items = res.data;
    });
  }

}
