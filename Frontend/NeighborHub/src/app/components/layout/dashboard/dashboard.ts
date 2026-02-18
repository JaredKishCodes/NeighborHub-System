import { Component, inject, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DashboardService } from '../../../services/dashboard/dashboard-service';
import { CurrentUserService } from '../../../services/current-user.service';
import { ItemService } from '../../../services/item.service';
import {
  DashboardData,
  DashboardBookingDto,
  DashboardLendingDto,
  BookingStatus,
} from '../../../models/dashboard.types';

@Component({
  selector: 'app-dashboard',
  imports: [NgClass, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private dashboardService = inject(DashboardService);
  private currentUserService = inject(CurrentUserService);
  private itemService = inject(ItemService);

  data: DashboardData | null = null;
  loading = true;
  error: string | null = null;

  createItemModalOpen = false;
  createItemSubmitting = false;
  createItemError: string | null = null;
  selectedFile: File | null = null;
  itemModel = {
    name: '',
    description: '',
    category: '',
    itemStatus: 0,
    createdAt: new Date(),
    ownerId: 1,
  };

  ngOnInit(): void {
    this.loadSummary();
  }

  loadSummary(): void {
    this.loading = true;
    this.error = null;
    const userId = this.currentUserService.getUserId();

    this.dashboardService.getSummary(userId).subscribe({
      next: (res) => {
        this.data = res;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message ?? 'Failed to load dashboard';
        this.loading = false;
      },
    });
  }

  get bookings(): DashboardBookingDto[] {
    return this.data?.bookingsThisMonth ?? [];
  }

  get lendings(): DashboardLendingDto[] {
    return this.data?.lendingsThisMonth ?? [];
  }

  formatDate(value: string): string {
    if (!value) return '—';
    const d = new Date(value);
    return d.toLocaleDateString(undefined, {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
    });
  }

  statusLabel(status: BookingStatus): string {
    return status ?? '—';
  }

  statusClass(status: BookingStatus): string {
    switch (status) {
      case 'Confirmed':
        return 'badge-success';
      case 'Completed':
        return 'badge-info';
      case 'Cancelled':
        return 'badge-error';
      default:
        return 'badge-warning';
    }
  }

  openCreateItemModal(): void {
    this.createItemModalOpen = true;
    this.createItemError = null;
    this.resetCreateItemForm();
    const dialog = document.getElementById('create-item-modal') as HTMLDialogElement;
    if (dialog) dialog.showModal();
  }

  closeCreateItemModal(): void {
    this.createItemModalOpen = false;
    this.createItemError = null;
    const dialog = document.getElementById('create-item-modal') as HTMLDialogElement;
    if (dialog) dialog.close();
  }

  onCreateItemFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] ?? null;
  }

  resetCreateItemForm(): void {
    this.itemModel = {
      name: '',
      description: '',
      category: '',
      itemStatus: 0,
      createdAt: new Date(),
      ownerId: this.currentUserService.getUserId() ?? 1,
    };
    this.selectedFile = null;
  }

  onCreateItemSubmit(): void {
    this.createItemError = null;
    if (!this.itemModel.name?.trim()) {
      this.createItemError = 'Item name is required.';
      return;
    }
    if (!this.itemModel.category?.trim()) {
      this.createItemError = 'Category is required.';
      return;
    }
    if (!this.selectedFile) {
      this.createItemError = 'Please choose an image for the item.';
      return;
    }
    this.createItemSubmitting = true;
    const payload = { ...this.itemModel, ownerId: this.currentUserService.getUserId() ?? this.itemModel.ownerId };
    this.itemService.createItem(payload, this.selectedFile).subscribe({
      next: () => {
        this.createItemSubmitting = false;
        this.closeCreateItemModal();
        this.loadSummary();
      },
      error: (err) => {
        this.createItemSubmitting = false;
        this.createItemError = err?.error?.message ?? err?.message ?? 'Failed to create item.';
      },
    });
  }
}
