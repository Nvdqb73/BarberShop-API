﻿using BarberShop.DTO;
using BarberShop.Entity;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceManagementController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ServiceManagementController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetServiceManagement()
        {
            var serviceManagement = _unitOfWork.ServiceManagementRepository.GetAll<ServiceManagementDto>();
            return Ok(serviceManagement);
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceManagement(int id)
        {
            var serviceManagement = _unitOfWork.ServiceManagementRepository.GetById<ServiceManagementDto>(id);

            if (serviceManagement == null)
                return NotFound();

            return Ok(serviceManagement);
        }

        [HttpPost]
        public IActionResult CreateServiceManagement(ServiceManagementDto serviceManagementModel)
        {
            if (serviceManagementModel == null)
                return BadRequest();

            var serviceManagementEntity = _unitOfWork.Mapper.Map<ServiceManagement>(serviceManagementModel);
            _unitOfWork.ServiceManagementRepository.Add(serviceManagementEntity);
            _unitOfWork.Commit();

            var serviceManagementDto = _unitOfWork.Mapper.Map<ServiceManagementDto>(serviceManagementEntity);

            return CreatedAtAction(nameof(GetServiceManagement), new { id = serviceManagementDto.serManagerID }, serviceManagementDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateServiceManagement(int id, ServiceManagementDto updatedServiceManagementModel)
        {
            var existingServiceManagementEntity = _unitOfWork.ServiceManagementRepository.GetById<ServiceManagementDto>(id);

            if (existingServiceManagementEntity == null)
                return NotFound();

            _unitOfWork.ServiceManagementRepository.UpdateProperties(id, entity =>
            {
                entity.storeID = updatedServiceManagementModel.storeID;
                entity.Service.serID = updatedServiceManagementModel.serID; 
            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteServiceManagement(int id)
        {
            var serviceManagementEntity = _unitOfWork.ServiceManagementRepository.GetById<ServiceManagementDto>(id);

            if (serviceManagementEntity == null)
                return NotFound();

            _unitOfWork.ServiceManagementRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
