import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { BookingService } from '../../../services/booking-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './booking.html',
  styleUrls: ['./booking.css'],
})
export class BookingComponent implements OnInit, OnChanges {
 

  availableDates: Date[] = [];
  @Input() item: number | null = null;

  startDate!: string; // YYYY-MM-DD
  endDate!: string;   // YYYY-MM-DD

  minDate: string; // Prevent past date selection

  constructor(private bookingService: BookingService) {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];
  }

  ngOnInit() {
    if (this.item) {
      this.loadAvailableDates();
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['item'] && !changes['item'].firstChange && this.item) {
      this.loadAvailableDates();
      // Reset dates when item changes
      this.startDate = '';
      this.endDate = '';
    }
  }

  loadAvailableDates() {
    if (!this.item) return;
    this.bookingService.getAvailableDates(this.item)
      .subscribe({
        next: (dates) => {
          this.availableDates = dates.map(d => new Date(d));
        },
        error: (error) => {
          console.error('Error loading available dates:', error);
          // Set empty array on error, dates will still be selectable
          this.availableDates = [];
        }
      });
  }

  // Ensure endDate cannot be before startDate
  onStartDateChange() {
    if (this.endDate && this.endDate < this.startDate) {
      this.endDate = this.startDate;
    }
  }

  onEndDateChange() {
    if (this.startDate && this.endDate < this.startDate) {
      alert('End date cannot be before start date');
      this.endDate = this.startDate;
    }
  }

  // Check if a date is available (optional for UI feedback)
  isDateAvailable(dateStr: string): boolean {
    const date = new Date(dateStr);
    return this.availableDates.some(d => d.toDateString() === date.toDateString());
  }

  bookItem() {
    console.log('Clicked item ID:', this.item);
  if (!this.startDate || !this.endDate) {
    alert('Please select both start and end dates');
    return;
  }
  if (!this.item) {
    alert('No item selected');
    return;
  }

  // Convert date strings to full ISO
  const startIso = new Date(this.startDate).toISOString();
  const endIso = new Date(this.endDate).toISOString();

  const bookingPayload = {
    itemId: this.item,
    borrowerId: 1, // replace with your current logged-in user's ID
    startDate: startIso,
    endDate: endIso
  };

  console.log(bookingPayload);
  

  this.bookingService.createBooking(bookingPayload)
    .subscribe({
      next: () => {
        alert('Booking successful!');
        this.loadAvailableDates();
        // Reset form
        this.startDate = '';
        this.endDate = '';
      },
      error: (error) => {
        console.error('Error creating booking:', error);
        alert('Failed to create booking. Please try again.');
      }
    });
}

onClick() {
    console.log('Clicked item ID:', this.item);
    // Handle the click using the ID, e.g., navigate or call a service
  }

}
