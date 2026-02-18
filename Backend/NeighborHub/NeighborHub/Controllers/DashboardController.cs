using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs.Dashboard;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DashboardController(IDashboardRepository _dashboardRepository) : ControllerBase
{
    [HttpGet("Summary")]
    public async Task<ActionResult<DashboardResponseDto>> GetSummary(int? userId = null)
    {
        IEnumerable<Booking> bookings = await _dashboardRepository.GetBookingsForCurrentMonthAsync(userId);
        IEnumerable<Booking> lendings = await _dashboardRepository.GetLendingsForCurrentMonthAsync(userId);

        var summary = new DashboardResponseDto
        {
           
            TotalItems = await _dashboardRepository.GetTotalItemsCountAsync(),
            TotalBookings = await _dashboardRepository.GetTotalBookingsCountAsync(userId),
            TotalLendings = await _dashboardRepository.GetTotalLendingsCountAsync(userId),
            BookingsThisMonth = bookings.Select(b => new DashboardBookingDto
            {
                Id = b.Id,
                ItemName = b.Item?.Name ?? "Unknown Item", // Validation against nulls
                OwnerName = b.Borrower?.FullName ?? "Unknown User",
                StartDate = b.StartDate,
                Status = b.BookingStatus
            }),

            LendingsThisMonth = lendings.Select(b => new DashboardLendingDto
            {
                Id = b.Id,
                ItemName = b.Item?.Name ?? "Unknown Item",
                BorrowerName = b.Borrower?.FullName ?? "Unknown User",
                StartDate = b.StartDate,
                Status = b.BookingStatus
            })



        };

        return Ok(summary);
    }

}
