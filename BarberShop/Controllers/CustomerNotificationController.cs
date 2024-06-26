﻿using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerNotificationController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerNotificationController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCustomerNotification()
        {
            var customerNotification = _unitOfWork.CustomerNotificationRepository.GetAll<CustomerNotificationDto>();
            return Ok(customerNotification);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerNotification(int id)
        {
            var customerNotification = _unitOfWork.CustomerNotificationRepository.GetById<CustomerNotificationDto>(id);

            if (customerNotification == null)
                return NotFound();

            return Ok(customerNotification);
        }

        [HttpPost]
        public IActionResult CreateCustomerNotification(CustomerNotificationDto customerNotificationModel)
        {
            if (customerNotificationModel == null)
                return BadRequest();

            var customerNotificationEntity = _unitOfWork.Mapper.Map<CustomerNotification>(customerNotificationModel);
            _unitOfWork.CustomerNotificationRepository.Add(customerNotificationEntity);
            _unitOfWork.Commit();

            var customerNotificationDto = _unitOfWork.Mapper.Map<CustomerNotificationDto>(customerNotificationEntity);

            return CreatedAtAction(nameof(GetCustomerNotification), new { id = customerNotificationDto.cNotificationID }, customerNotificationDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomerNotification(int id, CustomerNotificationDto updatedCustomerNotificationModel)
        {
            var existingCustomerNotificationEntity = _unitOfWork.CustomerNotificationRepository.GetById<CustomerNotificationDto>(id);

            if (existingCustomerNotificationEntity == null)
                return NotFound();

            _unitOfWork.CustomerNotificationRepository.UpdateProperties(id, entity =>
            {
                entity.Customer.customerID = updatedCustomerNotificationModel.customerID;
                entity.Notification.notiID = updatedCustomerNotificationModel.notiID;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerNotification(int id)
        {
            var customerNotificationEntity = _unitOfWork.CustomerNotificationRepository.GetById<CustomerNotificationDto>(id);

            if (customerNotificationEntity == null)
                return NotFound();

            _unitOfWork.CustomerNotificationRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
