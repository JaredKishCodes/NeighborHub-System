import { Booking } from "./booking.model";
import { Item } from "./item.model";

export interface DomainUser {
  id?: number;
  identityId?: string;
  fullName?: string;

  // Navigation (OPTIONAL – usually avoid in frontend)
  ownedItems?: Item[];
  borrowedBookings?: Booking[];
}