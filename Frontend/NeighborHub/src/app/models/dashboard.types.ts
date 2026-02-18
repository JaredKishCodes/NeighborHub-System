export type BookingStatus = 'Requested' | 'Confirmed' | 'Completed' | 'Cancelled';

export interface DashboardBookingDto {
  id: number;
  itemName: string;
  ownerName: string;
  startDate: string;
  status: BookingStatus;
}

export interface DashboardLendingDto {
  id: number;
  itemName: string;
  borrowerName: string;
  startDate: string;
  status: BookingStatus;
}

export interface DashboardData {
  totalItems: number;
  totalBookings: number;
  totalLendings: number;
  bookingsThisMonth: DashboardBookingDto[];
  lendingsThisMonth: DashboardLendingDto[];
}
