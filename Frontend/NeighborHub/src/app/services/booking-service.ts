import { inject, Injectable } from '@angular/core';
import { env } from '../../environments/environment.production';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class BookingService {

    private apiUrl = env.apiBaseUrl;

    private http  = inject(HttpClient);

    getAvailableDates(itemId: number) {
    return this.http.get<Date[]>(
     this.apiUrl + `/api/item/${itemId}/available-dates`
    );
  }

  createBooking(data: any) {
    return this.http.post(this.apiUrl+ '/api/Booking/create-booking', data);
  }
}
