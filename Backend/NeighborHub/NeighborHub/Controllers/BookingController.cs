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

        if (bookings == null || !bookings.Any())
        {
            return NotFound(new ApiResponse<IEnumerable<BookingResponseDto>>
            {
                Success = false,
                Message = "No borrowing records found for this user.",
                Data = null
            });
        }

        return Ok(new ApiResponse<IEnumerable<BookingResponseDto>>
        {
            Success = true,
            Message = "Borrowing bookings retrieved successfully.",
            Data = bookings
        });
    }

    [HttpGet("my-lendings/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookingResponseDto>>>> GetMyLendingsAsync(int userId)
    {
        IEnumerable<BookingResponseDto> bookings = await _bookingService.GetMyLendingAsync(userId);

        if (bookings == null || !bookings.Any())
        {
            return NotFound(new ApiResponse<IEnumerable<BookingResponseDto>>
            {
                Success = false,
                Message = "No lending records found for this user.",
                Data = null
            });
        }

        return Ok(new ApiResponse<IEnumerable<BookingResponseDto>>
        {
            Success = true,
            Message = "Lending bookings retrieved successfully.",
            Data = bookings
        });
    }

    [HttpGet("{bookingId}", Name = "GetBookingByIdAsync")]
    public async Task<ActionResult<ApiResponse<BookingResponseDto>>> GetBookingByIdAsync(int bookingId)
    {
        BookingResponseDto booking = await _bookingService.GetBookingByIdAsync(bookingId);

        if (booking == null)
        {
            return NotFound(new ApiResponse<BookingResponseDto>
            {
                Success = false,
                Message = $"Booking with ID {bookingId} was not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<BookingResponseDto>
        {
            Success = true,
            Message = "Booking retrieved successfully.",
            Data = booking
        });
    }

    [HttpPost("create-booking")]
    public async Task<ActionResult<ApiResponse<BookingResponseDto>>> CreateBookingAsync([FromBody] CreateBookingDto createBookingDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<BookingResponseDto>
            {
                Success = false,
                Message = "Invalid booking data provided.",
                Data = null
            });
        }

        BookingResponseDto result = await _bookingService.CreateBookingAsync(createBookingDto);

        return CreatedAtRoute(
            "GetBookingByIdAsync",
            new { bookingId = result.Id },
            new ApiResponse<BookingResponseDto>
            {
                Success = true,
                Message = "Booking created successfully.",
                Data = result
            });
    }

    [HttpPut("update-booking/{bookingId}")]
    public async Task<ActionResult<ApiResponse<BookingResponseDto>>> UpdateBookingAsync(int bookingId, [FromBody] UpdateBookingDto updateBookingDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<BookingResponseDto>
            {
                Success = false,
                Message = "Invalid update data.",
                Data = null
            });
        }

        BookingResponseDto updatedBooking = await _bookingService.UpdateBookingAsync(bookingId, updateBookingDto);

        if (updatedBooking == null)
        {
            return NotFound(new ApiResponse<BookingResponseDto>
            {
                Success = false,
                Message = "Booking not found or update failed.",
                Data = null
            });
        }

        return Ok(new ApiResponse<BookingResponseDto>
        {
            Success = true,
            Message = "Booking updated successfully.",
            Data = updatedBooking
        });
    }

    [HttpDelete("{bookingId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBookingAsync(int bookingId)
    {
        bool deleted = await _bookingService.DeleteBookingAsync(bookingId);

        if (!deleted)
        {
            return NotFound(new ApiResponse<bool>
            {
                Success = false,
                Message = "Booking delete was unsuccessful. ID might not exist.",
                Data = false
            });
        }

        return Ok(new ApiResponse<bool>
        {
            Success = true,
            Message = "Booking deleted successfully.",
            Data = true
        });
    }
}
