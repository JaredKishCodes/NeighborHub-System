using NeighborHub.Domain.Enums;

namespace NeighborHub.Application.DTOs.Dashboard;

public class DashboardResponseDto
{
  
    public int TotalItems { get; set; }
    public int TotalBookings { get; set; }
    public int TotalLendings { get; set; }

    
    public IEnumerable<DashboardBookingDto> BookingsThisMonth { get; set; }
    public IEnumerable<DashboardLendingDto> LendingsThisMonth { get; set; }
}


public class DashboardLendingDto
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string BorrowerName { get; set; }
    public DateTime StartDate { get; set; } 
    public BookingStatus Status { get; set; }
}

public class DashboardBookingDto
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string OwnerName { get; set; }
    public DateTime StartDate { get; set; }
    public BookingStatus Status { get; set; }
}
