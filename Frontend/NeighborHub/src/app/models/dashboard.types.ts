
export interface Transaction {
  id: number;
  itemName: string;
  borrowerName: string;
  startDate: string;
  status: 'Requested' | 'Approved' | 'Declined' | 'Returned' | 'Overdue';
}

export interface DashboardData {
  totalItems: number;
  totalBookings: number;
  totalLendings: number;
  bookingsThisMonth: Transaction[];
  lendingsThisMonth: Transaction[];
}