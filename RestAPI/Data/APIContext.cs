using Microsoft.EntityFrameworkCore;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.Data
{
    public class APIContext : DbContext
    {
        public DbSet<HotelBooking> Bookings { get; set; }

        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }
    }
}
