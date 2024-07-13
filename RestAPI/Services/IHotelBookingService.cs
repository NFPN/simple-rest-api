using RestAPI.Models;

namespace RestAPI.Services
{
    public interface IHotelBookingService
    {
        IEnumerable<HotelBooking> GetAll();

        HotelBooking? Get(int id);

        bool AddNew(HotelBooking booking);

        bool Update(HotelBooking booking);

        bool Delete(int id);
    }
}
