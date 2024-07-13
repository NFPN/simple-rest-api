using Microsoft.AspNetCore.Mvc;
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly APIContext context;

        public HotelBookingController(APIContext apiContext)
        {
            context = apiContext;
        }

        //CREATE/EDIT
        [HttpPost]
        public JsonResult CreateEdit(HotelBooking booking)
        {
            if (booking.Id == 0)
            {
                context.Bookings.Add(booking);
            }
            else
            {
                var bookingInDB = context.Bookings.Find(booking.Id);

                if (bookingInDB == null) return new JsonResult(NotFound());

                bookingInDB = booking;
            }

            context.SaveChanges();

            return new JsonResult(Ok(booking));
        }

        //GET
        [HttpGet]
        public JsonResult Get(int id)
        {
            var bookingInDB = context.Bookings.Find(id);

            if (bookingInDB == null) return new JsonResult(NotFound());

            context.SaveChanges();

            return new JsonResult(Ok(bookingInDB));
        }

        //DELETE
        [HttpGet]
        public JsonResult Delete(int id)
        {
            var bookingInDB = context.Bookings.Find(id);

            if (bookingInDB == null) return new JsonResult(NotFound());

            context.Bookings.Remove(bookingInDB);
            context.SaveChanges();

            return new JsonResult(Ok(NoContent()));
        }

        //GET ALL
        [HttpGet]
        public JsonResult GetAll()
        {
            return new JsonResult(Ok(context.Bookings.ToList()));
        }
    }
}
