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
  selector: 'app-my-lendings',
  standalone: true,
  imports: [NgClass],
  templateUrl: './my-lendings.html',
  styleUrl: './my-lendings.css',
})
export class MyLendings implements OnInit {
  private bookingService = inject(BookingService);
  private currentUserService = inject(CurrentUserService);
  private cdr = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  lendings: BookingListItem[] = [];
  loading = true;
  error: string | null = null;
  updatingId: number | null = null;

  ngOnInit(): void {
    this.loadLendings();
  }

  loadLendings(): void {
    const userId = this.currentUserService.getUserId();
    if (userId == null) {
      this.error = 'Please log in to view your lendings.';
      this.loading = false;
      return;
    }

    this.loading = true;
    this.error = null;

    this.bookingService.getMyLendings(userId).subscribe({
      next: (res) => {
        this.ngZone.run(() => {
          this.lendings = res.data ?? [];
          this.loading = false;
          this.cdr.markForCheck();
        });
      },
      error: (err) => {
        this.ngZone.run(() => {
          this.error = err?.error?.message ?? err?.message ?? 'Failed to load lendings';
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

  canRespond(status: BookingStatusLabel): boolean {
    return status === 'Requested';
  }

  confirmBooking(booking: BookingListItem): void {
    this.updateStatus(booking, BookingStatus.Confirmed);
  }

  declineBooking(booking: BookingListItem): void {
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
          this.loadLendings();
        },
        error: (err) => {
          this.updatingId = null;
          this.error = err?.error?.message ?? err?.message ?? 'Failed to update booking';
          this.cdr.markForCheck();
        },
      });
  }
}
