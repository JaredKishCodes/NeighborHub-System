import { Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';
import { ItemService } from '../../../services/item.service';
import { ApiResponse, ItemResponse, ItemStatus } from '../../../models/item.model';
import { CommonModule } from '@angular/common';
import { env } from '../../../../environments/environment.production';
import { FormsModule } from '@angular/forms';
import { BookingComponent } from '../booking/booking';

const PAGE_SIZE = 8;

@Component({
  selector: 'app-item',
  imports: [CommonModule, NgClass, FormsModule, BookingComponent],
  templateUrl: './item.html',
  styleUrl: './item.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class Item implements OnInit {
  itemService = inject(ItemService);
  readonly baseUrl = env.apiBaseUrl;

  items: ItemResponse[] = [];
  selectedItemId: number | null = null;

  currentPage = 1;
  pageSize = PAGE_SIZE;

  ngOnInit(): void {
    this.itemService.getItems().subscribe((res: ApiResponse<ItemResponse[]>) => {
      this.items = res.data ?? [];
    });
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.items.length / this.pageSize));
  }

  get paginatedItems(): ItemResponse[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.items.slice(start, start + this.pageSize);
  }

  get startIndex(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get endIndex(): number {
    return Math.min(this.currentPage * this.pageSize, this.items.length);
  }

  prevPage(): void {
    if (this.currentPage > 1) this.currentPage--;
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  openBookingModal(itemId: number): void {
    this.selectedItemId = itemId;
    const modal = document.getElementById('my_modal_3') as HTMLDialogElement;
    if (modal) modal.showModal();
  }

  statusLabel(status: ItemStatus): string {
    return ItemStatus[status] ?? 'Unknown';
  }

  statusBadgeClass(status: ItemStatus): string {
    switch (status) {
      case ItemStatus.Available:
        return 'item-badge-available';
      case ItemStatus.Requested:
        return 'item-badge-requested';
      case ItemStatus.Borrowed:
        return 'item-badge-borrowed';
      default:
        return '';
    }
  }
}
