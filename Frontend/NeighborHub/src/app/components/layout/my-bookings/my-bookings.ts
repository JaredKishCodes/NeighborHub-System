import { ChangeDetectorRef, Component, inject, NgZone, OnInit } from '@angular/core';
import { NgClass } from '@angular/common';
import { BookingService } from '../../../services/booking-service';
import { CurrentUserService } from '../../../services/current-user.service';
import {
  BookingListItem,
  BookingStatus,
  BookingStatusLabel,
} from '../../../models/booking.model';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [NgClass],
  templateUrl: './my-bookings.html',
  styleUrl: './my-bookings.css',
})
export class MyBookings implements OnInit {
  private bookingService = inject(BookingService);
  private currentUserService = inject(CurrentUserService);
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  bookings: BookingListItem[] = [];
  loading = true;
  error: string | null = null;
  updatingId: number | null = null;

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    const userId = this.currentUserService.getUserId();
    if (userId == null) {
      this.error = 'Please log in to view your bookings.';
      this.loading = false;
      return;
    }

    this.loading = true;
    this.error = null;

    this.bookingService.getMyBorrowings(userId).subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.bookings = res.data ?? [];
          this.loading = false;
          this.cdr.markForCheck();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.error = err?.error?.message ?? err?.message ?? 'Failed to load bookings';
          this.loading = false;
          this.cdr.markForCheck();
        });
      },
    });
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

  statusLabel(status: BookingStatusLabel): string {
    return status ?? '—';
  }

  statusClass(status: BookingStatusLabel): string {
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

  canCancel(status: BookingStatusLabel): boolean {
    return status === 'Requested';
  }

  cancelBooking(booking: BookingListItem): void {
    this.updateStatus(booking, BookingStatus.Cancelled);
  }

  private updateStatus(booking: BookingListItem, status: BookingStatus): void {
    this.updatingId = booking.id;
    this.bookingService
      .updateBooking(booking.id, {
        id: booking.id,
        bookingStatus: status,
        endDate: booking.endDate,
      })
      .subscribe({
        next: () => {
          this.updatingId = null;
          this.loadBookings();
        },
        error: (err) => {
          this.updatingId = null;
          this.error = err?.error?.message ?? err?.message ?? 'Failed to update booking';
          this.cdr.markForCheck();
        },
      });
  }
}
