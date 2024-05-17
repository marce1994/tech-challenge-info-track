using System;
using System.Collections.Generic;
using System.Linq;
namespace Settlement.Application.Exceptions;

public class BookingNotFoundException : Exception
{
    public BookingNotFoundException() : base("Booking not found")
    {
    }
}