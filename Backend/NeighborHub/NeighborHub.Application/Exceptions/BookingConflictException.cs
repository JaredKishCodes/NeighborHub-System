using System;

namespace NeighborHub.Application.Exceptions;

public class BookingConflictException : Exception
{

    public BookingConflictException(string message) : base(message)
    {
    }

    // Optional: Useful if you want to pass more data, like the conflicting dates
    public BookingConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
