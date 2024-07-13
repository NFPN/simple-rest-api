using Microsoft.AspNetCore.Mvc;
using Moq;
using RestAPI.Controllers;
using RestAPI.Models;
using RestAPI.Services;

namespace RestAPI.Tests
{
    public class HotelBookingControllerTests
    {
        private readonly Mock<IHotelBookingService> mockService;
        private readonly HotelBookingController controller;

        public HotelBookingControllerTests()
        {
            mockService = new Mock<IHotelBookingService>();
            controller = new HotelBookingController(mockService.Object);
        }

        [Fact]
        public void CreateOrEdit_ShouldReturnCreated_WhenBookingIsNew()
        {
            // Arrange
            var booking = new HotelBooking { Id = 0, Room = 101, CustomerName = "John Doe" };
            mockService.Setup(s => s.AddNew(booking)).Returns(true);

            // Act
            var result = controller.CreateOrEdit(booking);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var createdResult = Assert.IsType<CreatedResult>(jsonResult.Value);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void CreateOrEdit_ShouldReturnConflict_WhenBookingAlreadyExists()
        {
            // Arrange
            var booking = new HotelBooking { Id = 0, Room = 101, CustomerName = "John Doe" };
            mockService.Setup(s => s.AddNew(booking)).Returns(false);

            // Act
            var result = controller.CreateOrEdit(booking);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var conflictResult = Assert.IsType<ConflictObjectResult>(jsonResult.Value);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public void Get_ShouldReturnOk_WhenBookingExists()
        {
            // Arrange
            var booking = new HotelBooking { Id = 1, Room = 101, CustomerName = "John Doe" };
            mockService.Setup(s => s.Get(1)).Returns(booking);

            // Act
            var result = controller.Get(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Get_ShouldReturnNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            mockService.Setup(s => s.Get(1)).Returns(null as HotelBooking);

            // Act
            var result = controller.Get(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(jsonResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithAllBookings()
        {
            // Arrange
            var bookings = new List<HotelBooking>
            {
                new() { Id = 1, Room = 101, CustomerName = "John Doe" },
                new() { Id = 2, Room = 102, CustomerName = "Jane Doe" }
            };
            mockService.Setup(s => s.GetAll()).Returns(bookings);

            // Act
            var result = controller.GetAll();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(jsonResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Delete_ShouldReturnOk_WhenBookingExists()
        {
            // Arrange
            mockService.Setup(s => s.Delete(1)).Returns(true);

            // Act
            var result = controller.Delete(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var okResult = Assert.IsType<OkResult>(jsonResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenBookingDoesNotExist()
        {
            // Arrange
            mockService.Setup(s => s.Delete(1)).Returns(false);

            // Act
            var result = controller.Delete(1);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(jsonResult.Value);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}