using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class APIContext : DbContext
    {
        public DbSet<HotelBooking> Bookings { get; set; }

        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }
    }
}
