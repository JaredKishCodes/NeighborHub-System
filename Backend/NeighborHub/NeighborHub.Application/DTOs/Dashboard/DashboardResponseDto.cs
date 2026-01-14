namespace NeighborHub.Application.DTOs.Dashboard;

public class DashboardResponseDto
{
    public int TotalUsers { get; set; }
    public int TotalItems { get; set; }
    public int TotalBookings { get; set; }
    public int TotalLendings { get; set; }

    
    public IEnumerable<DashboardBookingDto> BookingsThisMonth { get; set; }
    public IEnumerable<DashboardBookingDto> LendingsThisMonth { get; set; }
}


public class DashboardBookingDto
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string BorrowerName { get; set; }
    public string StartDate { get; set; } 
    public string Status { get; set; }
}
