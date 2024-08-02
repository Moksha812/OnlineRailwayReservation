using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;
using OnlineRailwayReservation.Repository;

namespace OnlineRailwayReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationRepository _stationRepository;
        private readonly IMapper _mapper;
        public StationController(IStationRepository stationRepository,IMapper mapper)
        {
            _stationRepository = stationRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStations()
        {
            try
            {
                var stations = await _stationRepository.GetAllStationsAsync();
                var stationDtos=_mapper.Map<IEnumerable<StationDto>>(stations);
                return Ok(stationDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStationById(int id)
        {
            try
            {
                var station = await _stationRepository.GetStationByIdAsync(id);
                if (station == null)
                {
                    return NotFound();
                }
                var stationDto=_mapper.Map<StationDto>(station);
                return Ok(stationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<IActionResult> CreateStation(CreateStationDto stationDto)
        {
            try
            {
                var station=_mapper.Map<Station>(stationDto);
                var res = await _stationRepository.AddStationAsync(station);
                var stationDtos = _mapper.Map<StationDto>(station);
                
                //var stationDto = new StationDto
                //{
                //    StationId = station.StationId,
                //    StationName = station.StationName,
                //    city = station.city,
                //    State = station.State,
                //};
                var ret = new
                {
                    success=res,
                    station=stationDtos??null
                };
                return CreatedAtAction(nameof(GetStationById), new { id = station.StationId }, ret);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStation(int id,[FromBody]UpdateStationDto updatestation)
        {
            try
            {
                if (id != updatestation.StationId)
                {
                    return BadRequest();
                }
                var station=_mapper.Map<Station>(updatestation);
                await _stationRepository.UpdateStationAsync(station);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            try
            {
                await _stationRepository.DeleteStationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
