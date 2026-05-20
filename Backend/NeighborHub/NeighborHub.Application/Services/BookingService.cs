using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Booking;
using NeighborHub.Application.Exceptions;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Enums;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IItemRepository _itemRepository;

    public BookingService(IBookingRepository bookingRepository, IItemRepository itemRepository)
    {
        _bookingRepository = bookingRepository;
        _itemRepository = itemRepository;
    }
    public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto createBookingDto)
    {
        if (createBookingDto.EndDate <= createBookingDto.StartDate)
        {
            throw new Exception("End date must be after the start date.");
        }

        bool hasOverlap = await _bookingRepository.HasOverlapAsync(createBookingDto.ItemId, createBookingDto.StartDate, createBookingDto.EndDate);

        if (hasOverlap)
        {
            throw new BookingConflictException("This item is already booked for these dates.");
        }
        var booking = new Booking
        {
            ItemId = createBookingDto.ItemId,
            BorrowerId = createBookingDto.BorrowerId,
            StartDate = createBookingDto.StartDate,
            EndDate = createBookingDto.EndDate,
            BookingStatus = BookingStatus.Requested,
        };

        await _bookingRepository.CreateBookingAsync(booking);

        Item item = await _itemRepository.GetItemById(createBookingDto.ItemId);
        if (item != null && item.ItemStatus == ItemStatus.available)
        {
            item.ItemStatus = ItemStatus.requested;
            await _itemRepository.UpdateItem(item);
        }

        return MapToDto(booking);
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        Booking booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

        if (booking != null)
        {
            await _bookingRepository.DeleteBookingAsync(bookingId);
            await SyncItemStatusFromBookingStateAsync(booking.ItemId);
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
        return MapToDto(booking);
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
        return bookings.Select(MapToDto).ToList();
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
        return bookings.Select(MapToDto).ToList();
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
        await SyncItemStatusFromBookingStateAsync(updatedBooking.ItemId);

        // Return the mapped DTO
        return MapToDto(updatedBooking);
    }

    private static BookingResponseDto MapToDto(Booking booking) => new()
    {
        Id = booking.Id,
        ItemId = booking.ItemId,
        BorrowerId = booking.BorrowerId,
        StartDate = booking.StartDate,
        EndDate = booking.EndDate,
        BookingStatus = booking.BookingStatus.ToString(),
        CreatedAt = booking.CreatedAt,
        ItemName = booking.Item?.Name ?? "Unknown Item",
        OwnerName = booking.Item?.Owner?.FullName ?? "Unknown Owner",
        BorrowerName = booking.Borrower?.FullName ?? "Unknown Borrower",
    };

    private async Task SyncItemStatusFromBookingStateAsync(int itemId)
    {
        Item item = await _itemRepository.GetItemById(itemId);
        if (item == null) return;

        List<Booking> itemBookings = await _bookingRepository.GetMyLendingAsync(item.OwnerId);
        List<Booking> relevantBookings = itemBookings
            .Where(b => b.ItemId == itemId)
            .ToList();

        if (relevantBookings.Any(b => b.BookingStatus == BookingStatus.Confirmed))
        {
            item.ItemStatus = ItemStatus.borrowed;
        }
        else if (relevantBookings.Any(b => b.BookingStatus == BookingStatus.Requested))
        {
            item.ItemStatus = ItemStatus.requested;
        }
        else
        {
            item.ItemStatus = ItemStatus.available;
        }

        await _itemRepository.UpdateItem(item);
    }
}
