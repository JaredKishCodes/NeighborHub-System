using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Booking;

namespace NeighborHub.Application.Interfaces;
public interface IBookingService
{
    Task<List<BookingResponseDto>> GetMyBorrowingAsync(int userId);
    Task<List<BookingResponseDto>> GetMyLendingAsync(int userId);
    Task<BookingResponseDto> GetBookingByIdAsync(int bookingId);
    Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto createBookingDto);
    Task<BookingResponseDto> UpdateBookingAsync(int bookingId, UpdateBookingDto updateBookingDto);
    Task<bool> DeleteBookingAsync(int bookingId);

}
