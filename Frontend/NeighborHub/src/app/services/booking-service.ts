import { inject, Injectable } from '@angular/core';
import { env } from '../../environments/environment.production';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/item.model';
import { BookingListItem, UpdateBookingPayload } from '../models/booking.model';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private apiUrl = env.apiBaseUrl;
  private http = inject(HttpClient);

  getAvailableDates(itemId: number) {
    return this.http.get<Date[]>(this.apiUrl + `/api/item/${itemId}/available-dates`);
  }

  createBooking(data: unknown) {
    return this.http.post(this.apiUrl + '/api/Booking/create-booking', data);
  }

  getMyBorrowings(userId: number): Observable<ApiResponse<BookingListItem[]>> {
    return this.http.get<ApiResponse<BookingListItem[]>>(
      `${this.apiUrl}/api/Booking/my-borrowings/${userId}`
    );
  }

  getMyLendings(userId: number): Observable<ApiResponse<BookingListItem[]>> {
    return this.http.get<ApiResponse<BookingListItem[]>>(
      `${this.apiUrl}/api/Booking/my-lendings/${userId}`
    );
  }

  updateBooking(
    bookingId: number,
    payload: UpdateBookingPayload
  ): Observable<ApiResponse<BookingListItem>> {
    return this.http.put<ApiResponse<BookingListItem>>(
      `${this.apiUrl}/api/Booking/update-booking/${bookingId}`,
      payload
    );
  }
}
