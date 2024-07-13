using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.Services;
using System.Net;
using System.Text;

namespace RestAPI.Tests
{
    public class HotelBookingControllerTests
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Program> factory;

        public HotelBookingControllerTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<APIContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDB");
                    });

                    services.AddScoped<IHotelBookingService, HotelBookingService>();
                });
            });

            client = this.factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            // Arrange
            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<APIContext>();
                context.Bookings.Add(new HotelBooking { Id = 1, Room = 101, CustomerName = "John Doe" });
                context.SaveChanges();
            }

            // Act
            var response = await client.GetAsync("/api/hotelbooking");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<HotelBooking>>(content);
            data.Should().HaveCount(1);
        }

        [Fact]
        public async Task Get_ReturnsNoContent()
        {
            // Act
            var response = await client.GetAsync("/api/hotelbooking/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task CreateOrUpdate_Create_ReturnsCreated()
        {
            // Arrange
            var newBooking = new HotelBooking { Id = 0, Room = 101, CustomerName = "Jane Doe" };
            var content = new StringContent(JsonConvert.SerializeObject(newBooking), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/hotelbooking", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var returnedBooking = JsonConvert.DeserializeObject<HotelBooking>(await response.Content.ReadAsStringAsync());
            returnedBooking.Room.Should().Be(newBooking.Room);
            returnedBooking.CustomerName.Should().Be(newBooking.CustomerName);
        }

        [Fact]
        public async Task CreateOrUpdate_Update_ReturnsOk()
        {
            // Arrange
            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<APIContext>();
                context.Bookings.Add(new HotelBooking { Id = 1, Room = 101, CustomerName = "John Doe" });
                context.SaveChanges();
            }

            var updatedBooking = new HotelBooking { Id = 1, Room = 102, CustomerName = "Jane Smith" };
            var content = new StringContent(JsonConvert.SerializeObject(updatedBooking), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/hotelbooking", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedBooking = JsonConvert.DeserializeObject<HotelBooking>(await response.Content.ReadAsStringAsync());
            returnedBooking.Should().BeEquivalentTo(updatedBooking);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<APIContext>();
                context.Bookings.Add(new HotelBooking { Id = 1, Room = 101, CustomerName = "John Doe" });
                context.SaveChanges();
            }

            // Act
            var response = await client.DeleteAsync("/api/hotelbooking/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}