using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Services
{
    public class HotelBookingService(APIContext apiContext) : IHotelBookingService
    {
        public IEnumerable<HotelBooking> GetAll()
           => [.. apiContext.Bookings];

        public HotelBooking? Get(int id) => apiContext.Bookings.Find(id);

        public bool AddNew(HotelBooking booking)
        {
            apiContext.Bookings.Add(booking);
            apiContext.SaveChanges();
            return true;
        }

        public bool Update(HotelBooking booking)
        {
            var toUpdate = apiContext.Bookings.Find(booking.Id);

            if (toUpdate == null)
                return false;

            toUpdate.Room = booking.Room;
            toUpdate.Id = booking.Id;
            apiContext.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var toDelete = apiContext.Bookings.Find(id);

            if (toDelete == null)
                return false;

            apiContext.Bookings.Remove(toDelete);
            apiContext.SaveChanges();
            return true;
        }
    }
}
