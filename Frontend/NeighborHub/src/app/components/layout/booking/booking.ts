import { Component, OnInit } from '@angular/core';
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
export class BookingComponent implements OnInit {

  availableDates: Date[] = [];
  itemId = 1;

  startDate!: string; // YYYY-MM-DD
  endDate!: string;   // YYYY-MM-DD

  minDate: string; // Prevent past date selection

  constructor(private bookingService: BookingService) {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];
  }

  ngOnInit() {
    this.loadAvailableDates();
  }

  loadAvailableDates() {
    this.bookingService.getAvailableDates(this.itemId)
      .subscribe(dates => {
        this.availableDates = dates.map(d => new Date(d));
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
  if (!this.startDate || !this.endDate) {
    alert('Please select both start and end dates');
    return;
  }

  // Convert date strings to full ISO
  const startIso = new Date(this.startDate).toISOString();
  const endIso = new Date(this.endDate).toISOString();

  const bookingPayload = {
    itemId: this.itemId,
    borrowerId: 1, // replace with your current logged-in user's ID
    startDate: startIso,
    endDate: endIso
  };

  console.log(bookingPayload);
  

  this.bookingService.createBooking(bookingPayload)
    .subscribe(() => {
      alert('Booking successful!');
      this.loadAvailableDates();
    });
}


}
