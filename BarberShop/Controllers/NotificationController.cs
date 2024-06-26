﻿using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public NotificationController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetNotification()
        {
            var notification = _unitOfWork.NotificationRepository.GetAll<NotificationDto>();
            return Ok(notification);
        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var notification = _unitOfWork.NotificationRepository.GetById<NotificationDto>(id);

            if (notification == null)
                return NotFound();

            return Ok(notification);
        }

        [HttpPost]
        public IActionResult CreateNotification(NotificationDto notificationModel)
        {
            if (notificationModel == null)
                return BadRequest();

            var notificationEntity = _unitOfWork.Mapper.Map<Notification>(notificationModel);
            _unitOfWork.NotificationRepository.Add(notificationEntity);
            _unitOfWork.Commit();

            var notificationDto = _unitOfWork.Mapper.Map<NotificationDto>(notificationEntity);

            return CreatedAtAction(nameof(GetNotification), new { id = notificationDto.notiID }, notificationDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(int id, NotificationDto updatedNotificationModel)
        {
            var existingNotificationEntity = _unitOfWork.NotificationRepository.GetById<NotificationDto>(id);

            if (existingNotificationEntity == null)
                return NotFound();

            _unitOfWork.NotificationRepository.UpdateProperties(id, entity =>
            {
                entity.notiTitle = updatedNotificationModel.notiTitle;
                entity.notiContent = updatedNotificationModel.notiContent;
                entity.lastUpdate = updatedNotificationModel.lastUpdate;
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var notificationEntity = _unitOfWork.NotificationRepository.GetById<NotificationDto>(id);

            if (notificationEntity == null)
                return NotFound();

            _unitOfWork.NotificationRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
