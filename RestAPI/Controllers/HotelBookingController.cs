using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Services;

namespace RestAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HotelBookingController(IHotelBookingService service) : ControllerBase
    {
        //CREATE/EDIT
        [HttpPost]
        public JsonResult CreateOrEdit(HotelBooking booking)
        {
            bool result;

            if (booking.Id == 0)
                result = service.AddNew(booking);
            else
                result = service.Update(booking);

            if (result == false) return booking.Id == 0 ?
                    new(Conflict(new { Reason = "Already Exists" }))
                    : new(NotFound(new { Reason = "Doesn't Exist" }));

            return new(Created("New entry added", booking));
        }

        //GET
        [HttpGet]
        public JsonResult Get(int id)
        {
            var booking = service.Get(id);

            return booking != null ?
                new(Ok(booking))
                : new(NotFound());
        }

        //GET ALL
        [HttpGet]
        public JsonResult GetAll()
            => new(Ok(service.GetAll()));

        //DELETE
        [HttpGet]
        public JsonResult Delete(int id)
            => service.Delete(id) ? new(Ok()) : new(NotFound());
    }
}
