using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class APIContext(DbContextOptions<APIContext> options) : DbContext(options)
    {
        public DbSet<HotelBooking> Bookings { get; set; }
    }
}
