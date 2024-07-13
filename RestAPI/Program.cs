// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Services;

namespace RestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<APIContext>(opt => opt.UseInMemoryDatabase("BookingDB"));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IHotelBookingService, HotelBookingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}