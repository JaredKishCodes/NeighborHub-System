using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Booking;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto createBookingDto)
    {
        var booking = new Booking
        {
            ItemId = createBookingDto.ItemId,
            BorrowerId = createBookingDto.BorrowerId,
            StartDate = createBookingDto.StartDate,
            EndDate = createBookingDto.EndDate,
        };

       await _bookingRepository.CreateBookingAsync(booking);

        return new BookingResponseDto
        {
            Id = booking.Id,
            ItemId = booking.ItemId,
            BorrowerId = booking.BorrowerId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            BookingStatus = booking.BookingStatus.ToString(),
            CreatedAt = booking.CreatedAt
        };
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

        if (booking != null)
        {
            await _bookingRepository.DeleteBookingAsync(bookingId);
            return true;
        }
        return false;

    }

    public async Task<BookingResponseDto> GetBookingByIdAsync(int bookingId)
    {
        Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking == null)
        {
            return null;
        }
        return new BookingResponseDto
        {
            Id = booking.Id,
            ItemId = booking.ItemId,
            BorrowerId = booking.BorrowerId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            BookingStatus = booking.BookingStatus.ToString(),
            CreatedAt = booking.CreatedAt
        };


    }

    public async Task<List<BookingResponseDto>> GetMyBorrowingAsync(int userId)
    {
        // Fetch the list of bookings for the user
        List<Booking> bookings = await _bookingRepository.GetMyBorrowingAsync(userId);

        // Check if there are no bookings and return an empty list
        if (bookings == null || !bookings.Any())
        {
            return new List<BookingResponseDto>();
        }

        // Map the bookings to BookingResponseDto and return the list
        return bookings.Select(booking => new BookingResponseDto
        {
            Id = booking.Id,
            ItemId = booking.ItemId,
            BorrowerId = booking.BorrowerId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            BookingStatus = booking.BookingStatus.ToString(),
            CreatedAt = booking.CreatedAt
        }).ToList();
    }

    public async Task<List<BookingResponseDto>> GetMyLendingAsync(int userId)
    {
        // Fetch the list of bookings for the user
        List<Booking> bookings = await _bookingRepository.GetMyLendingAsync(userId);

        // Check if there are no bookings and return an empty list
        if (bookings == null || !bookings.Any())
        {
            return new List<BookingResponseDto>();
        }

        // Map the bookings to BookingResponseDto and return the list
        return bookings.Select(booking => new BookingResponseDto
        {
            Id = booking.Id,
            ItemId = booking.ItemId,
            BorrowerId = booking.BorrowerId,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            BookingStatus = booking.BookingStatus.ToString(),
            CreatedAt = booking.CreatedAt
        }).ToList();
    }

    public async Task<BookingResponseDto> UpdateBookingAsync(int bookingId, UpdateBookingDto updateBookingDto)
    {
        // Fetch the booking
        Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId) ?? throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

        // Update fields (assuming UpdateBookingDto has these properties)
        booking.BookingStatus = updateBookingDto.BookingStatus;
        booking.EndDate = updateBookingDto.EndDate;

        // Save the updated booking
        Booking updatedBooking = await _bookingRepository.UpdateBookingAsync(booking);  // Note: Renamed to UpdateBookingAsync for consistency

        // Return the mapped DTO
        return new BookingResponseDto
        {
            Id = updatedBooking.Id,
            ItemId = updatedBooking.ItemId,
            BorrowerId = updatedBooking.BorrowerId,
            StartDate = updatedBooking.StartDate,
            EndDate = updatedBooking.EndDate,
            BookingStatus = updatedBooking.BookingStatus.ToString(),
            CreatedAt = updatedBooking.CreatedAt
        };
    }
}
