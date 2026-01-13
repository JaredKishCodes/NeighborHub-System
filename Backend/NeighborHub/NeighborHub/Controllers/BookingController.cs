using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs;
using NeighborHub.Application.DTOs.Booking;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("my-borrowings/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookingResponseDto>>>> GetMyBorrowingsAsync(int userId)
    {
        IEnumerable<BookingResponseDto> bookings = await _bookingService.GetMyBorrowingAsync(userId);
        if (bookings == null || !bookings.Any()) // Fix: Use bookings.Any() to check if the collection has elements
        {
            return NotFound(new ApiResponse<IEnumerable<BookingResponseDto>>
            {
                Success = false,
                Message = "Borrowing bookings retrieve was unsuccessfull.",
                Data = null
            });
        }
        return Ok(
            new ApiResponse<IEnumerable<BookingResponseDto>>
            {
                Success = true,
                Message = "Borrowing bookings retrieved successfully.",
                Data = bookings
            });
    }
}
