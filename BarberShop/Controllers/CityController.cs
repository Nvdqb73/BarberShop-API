using BarberShop.Entity;
using BarberShop.DTO;
using BarberShop.Unit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarberShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CityController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetCity()
        {
            var city = _unitOfWork.CityRepository.GetAll<CityDto>();
            return Ok(city);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = _unitOfWork.CityRepository.GetById<CityDto>(id);

            if (city == null)
                return NotFound();

            return Ok(city);
        }

        [HttpPost]
        public IActionResult CreateCity(CityDto cityModel)
        {
            if (cityModel == null)
                return BadRequest();

            var cityEntity = _unitOfWork.Mapper.Map<City>(cityModel);
            _unitOfWork.CityRepository.Add(cityEntity);
            _unitOfWork.Commit();

            var cityDto = _unitOfWork.Mapper.Map<CityDto>(cityEntity);

            return CreatedAtAction(nameof(GetCity), new { id = cityDto.cityID }, cityDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, CityDto updatedCityModel)
        {
            var existingCityEntity = _unitOfWork.CityRepository.GetById<CityDto>(id);

            if (existingCityEntity == null)
                return NotFound();

            _unitOfWork.CityRepository.UpdateProperties(id, entity =>
            {
                entity.cityName = updatedCityModel.cityName;
                entity.countryID = updatedCityModel.countryID;

            });

            _unitOfWork.Commit();

            return Ok(new { message = "Cập nhật thành công" });
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var cityEntity = _unitOfWork.CityRepository.GetById<CityDto>(id);

            if (cityEntity == null)
                return NotFound();

            _unitOfWork.CityRepository.Delete(id);
            _unitOfWork.Commit();

            return Ok(new { message = "Xóa thành công" });
        }


        [HttpGet("search")]
        public IActionResult SearchCitys([FromQuery] string citykey)
        {
            var cityes = _unitOfWork.CityRepository.Search<CityDto>(city =>
                            city.cityName != null &&
                            city.cityName.Contains(citykey));
            return Ok(cityes);
        }
    }
}
