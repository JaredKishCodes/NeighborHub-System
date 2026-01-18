import { DomainUser } from "./domainUser.model";
import { Item } from "./item.model";


export interface Booking {
  id: number;
  itemId: number;
  borrowerId: number;

  bookingStatus: BookingStatus;

  // Dates come as ISO strings from API
  startDate: string;
  endDate: string;
  createdAt: string;

  // Navigation (OPTIONAL – avoid unless needed)
  item?: Item;
  borrower?: DomainUser;
}

// src/app/models/booking-status.enum.ts
export enum BookingStatus {
  Requested = 0,
  Confirmed = 1,
  Completed = 2,
  Cancelled = 3
}
