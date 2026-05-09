import { ChangeDetectorRef, Component, CUSTOM_ELEMENTS_SCHEMA, inject, NgZone, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';
import { ItemService } from '../../../services/item.service';
import { ApiResponse, ItemResponse, ItemStatus } from '../../../models/item.model';
import { CommonModule } from '@angular/common';
import { env } from '../../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { BookingComponent } from '../booking/booking';

const PAGE_SIZE = 8;

@Component({
  selector: 'app-item',
  standalone: true,
  imports: [CommonModule, NgClass, FormsModule, BookingComponent],
  templateUrl: './item.html',
  styleUrl: './item.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class Item implements OnInit {
  itemService = inject(ItemService);
  readonly apiBaseUrl = env.apiBaseUrl;
  readonly imageBaseUrl = env.imageBaseUrl;
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  items: ItemResponse[] = [];
  selectedItemId: number | null = null;

  currentPage = 1;
  pageSize = PAGE_SIZE;

  ngOnInit(): void {
    this.itemService.getItems().subscribe({
      next: (res: ApiResponse<ItemResponse[]>) => {
        this.ngZone.run(() => {
          this.items = res.data ?? [];
          this.cdr.markForCheck();
        });
      },
      error: (err) => {
        console.error('Error loading items:', err);
        this.ngZone.run(() => {
          this.cdr.markForCheck();
        });
      },
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

  resolveImageUrl(imageUrl?: string): string {
    if (!imageUrl) return '';
    if (/^https?:\/\//i.test(imageUrl)) return imageUrl;
    if (imageUrl.startsWith('/')) return `${this.apiBaseUrl}${imageUrl}`;
    return `${this.apiBaseUrl}/${imageUrl}`;
  }

  statusLabel(status: ItemStatus): string {
    return ItemStatus[status] ?? 'Unknown';
  }

  statusBadgeClass(status: ItemStatus): string {
    switch (status) {
      case ItemStatus.available:
        return 'item-badge-available';
      case ItemStatus.requested:
        return 'item-badge-requested';
      case ItemStatus.borrowed:
        return 'item-badge-borrowed';
      default:
        return '';
    }
  }
}
