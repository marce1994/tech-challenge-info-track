using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Settlement.Api.Controllers;
using Settlement.Api.ViewModels;
using Settlement.Application.Exceptions;
using Settlement.Application.Models;
using Settlement.Application.Services;

namespace Settlement.Test.Controllers;

public class BookingControllerTest
{
    [Fact]
    public async void GetBooking_ShouldReturnBookings()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        var bookingViewModel = new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        };
        bookingService.Setup(x => x.GetBookings()).ReturnsAsync(new List<Booking> { booking });
        mapper.Setup(x => x.Map<IEnumerable<BookingViewModel>>(It.IsAny<IEnumerable<Booking>>())).Returns(new List<BookingViewModel> { bookingViewModel });
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.GetBookings()).Result as OkObjectResult;
        var bookings = result?.Value as IEnumerable<BookingViewModel>;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(bookings);
        Assert.Single(bookings);
        Assert.Equal(booking.Id, bookings.First().BookingId);
        Assert.Equal(booking.Name, bookings.First().Name);
        Assert.Equal(booking.BookingTime, bookings.First().BookingTime);
    }

    [Fact]
    public async void GetBooking_ShouldReturnBooking()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        var bookingViewModel = new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        };
        bookingService.Setup(x => x.GetBooking(booking.Id)).ReturnsAsync(booking);
        mapper.Setup(x => x.Map<BookingViewModel>(It.IsAny<Booking>())).Returns(bookingViewModel);
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.GetBooking(booking.Id)).Result as OkObjectResult;
        var bookingResult = result?.Value as BookingViewModel;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(bookingResult);
        Assert.Equal(booking.Id, bookingResult.BookingId);
        Assert.Equal(booking.Name, bookingResult.Name);
        Assert.Equal(booking.BookingTime, bookingResult.BookingTime);
    }

    [Fact]
    public async void GetBooking_ShouldReturnNotFound_WhenBookingIsNotFound()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        bookingService.Setup(x => x.GetBooking(It.IsAny<Guid>())).ThrowsAsync(new BookingNotFoundException());  
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.GetBooking(Guid.NewGuid())).Result as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async void CreateBooking_ShouldCreateBooking()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        var bookingViewModel = new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        };
        bookingService.Setup(x => x.CreateBooking(It.IsAny<Booking>())).ReturnsAsync(booking);
        mapper.Setup(x => x.Map<BookingViewModel>(It.IsAny<Booking>())).Returns(bookingViewModel);
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.CreateBooking(bookingViewModel)).Result as OkObjectResult;
        var bookingResult = result?.Value as BookingViewModel;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(bookingResult);
        Assert.Equal(booking.Id, bookingResult.BookingId);
        Assert.Equal(booking.Name, bookingResult.Name);
        Assert.Equal(booking.BookingTime, bookingResult.BookingTime);
    }

    [Fact]
    public async void CreateBooking_ShouldReturnBadRequest_WhenBookingConflictExceptionIsThrown()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        var bookingViewModel = new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        };
        bookingService.Setup(x => x.CreateBooking(It.IsAny<Booking>())).ThrowsAsync(new BookingConflictException());
        mapper.Setup(x => x.Map<Booking>(It.IsAny<BookingViewModel>())).Returns(booking);
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.CreateBooking(bookingViewModel)).Result as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Booking time conflict", result.Value);
    }

    [Fact]
    public async void CreateBooking_ShouldReturnBadRequest_WhenBookingBusinessHoursExceptionIsThrown()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        var bookingViewModel = new BookingViewModel
        {
            BookingId = booking.Id,
            Name = booking.Name,
            BookingTime = booking.BookingTime
        };
        bookingService.Setup(x => x.CreateBooking(It.IsAny<Booking>())).ThrowsAsync(new BookingBusinessHoursException());
        mapper.Setup(x => x.Map<Booking>(It.IsAny<BookingViewModel>())).Returns(booking);
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.CreateBooking(bookingViewModel)).Result as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Booking outside business hours", result.Value);
    }

    [Fact]
    public async void DeleteBooking_ShouldDeleteBooking()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        bookingService.Setup(x => x.DeleteBooking(booking.Id)).Returns(Task.CompletedTask);
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.DeleteBooking(booking.Id)) as NoContentResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async void DeleteBooking_ShouldReturnNotFound_WhenBookingNotFoundExceptionIsThrown()
    {
        // Arrange
        var bookingService = new Mock<IBookingService>();
        var mapper = new Mock<IMapper>();
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            BookingTime = new TimeOnly(9, 0, 0)
        };
        bookingService.Setup(x => x.DeleteBooking(booking.Id)).ThrowsAsync(new BookingNotFoundException());
        var controller = new BookingController(bookingService.Object, mapper.Object);

        // Act
        var result = (await controller.DeleteBooking(booking.Id)) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }
}