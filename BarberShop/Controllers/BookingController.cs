using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetBooking()
        {
            var booking = _unitOfWork.BookingRepository.GetAll<BookingDto>();
            return Ok(booking);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking(BookingDto bookingModel)
        {
            if (bookingModel == null)
                return BadRequest();

            var bookingEntity = _unitOfWork.Mapper.Map<Booking>(bookingModel);
            _unitOfWork.BookingRepository.Add(bookingEntity);
            _unitOfWork.Commit();

            var bookingDto = _unitOfWork.Mapper.Map<BookingDto>(bookingEntity);

            return CreatedAtAction(nameof(GetBooking), new { id = bookingDto.bookingID }, bookingDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, BookingDto updatedBookingModel)
        {
            var existingBookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (existingBookingEntity == null)
                return NotFound();

            _unitOfWork.BookingRepository.UpdateProperties(id, entity =>
            {
                entity.startTime = updatedBookingModel.startTime;
                entity.customer.customerID = updatedBookingModel.customerID;
                //entity.payment.payID = updatedBookingModel.payID;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var bookingEntity = _unitOfWork.BookingRepository.GetById<BookingDto>(id);

            if (bookingEntity == null)
                return NotFound();

            _unitOfWork.BookingRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
