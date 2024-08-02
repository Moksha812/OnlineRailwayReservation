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
    public class TrainController : ControllerBase
    {
        private readonly ITrainRepository _trainRepository;
        private readonly IMapper _mapper;
        public TrainController(ITrainRepository trainRepository,IMapper mapper)
        {
            _trainRepository = trainRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainDto>>> GetTrains()
        {
            try
            {
                var trains = await _trainRepository.GetAllTrains();
                //var trainDTOs = _mapper.Map<IEnumerable<TrainDto>>(trains);
                return Ok(trains);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainDto>> GetTrainById(int id)
        {
            try
            {
                var train = await _trainRepository.GetTrainById(id);
                if (train == null)
                {
                    return NotFound();
                }
                //var trainDTO = _mapper.Map<TrainDto>(train);
                return Ok(train);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        public async Task<ActionResult<TrainDto>> AddTrain(TrainDto trainDTO)
        {
            try
            {
                var train = _mapper.Map<Train>(trainDTO);
                train = await _trainRepository.AddTrain(train);
                return CreatedAtAction(nameof(GetTrainById), new { id = train.Train_Id }, trainDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrain(int id, TrainDto trainDTO)
        {
            var existingTrain = await _trainRepository.GetTrainById(id);
            if (existingTrain == null)
            {
                return BadRequest($"No train by id: {id} exist");
            }
            try
            {
                await _trainRepository.UpdateTrain(id, trainDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return NoContent();
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrain(int id)
        {
            try
            {
                var train = await _trainRepository.GetTrainById(id);
                if (train == null)
                {
                    return NotFound();
                }
                await _trainRepository.DeleteTrain(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("SearchTrains")]
        public async Task<IActionResult> GetTrainsBySourceAndDestinationStations([FromBody] SearchStationDto searchStationDto)
        {
            if (searchStationDto == null || searchStationDto.SourceStation == null || searchStationDto.DestinationStation == null)
                return BadRequest("Input entries incorrect");
            if (searchStationDto.SourceStation == searchStationDto.DestinationStation)
                return BadRequest("Source and destination station can not be same");
            try
            {
                var res = await _trainRepository.GetTrainsBySourceAndDestinationStations(searchStationDto.SourceStation, searchStationDto.DestinationStation,searchStationDto.TravelDate);
                if (res == null) return NotFound($"No trains found from {searchStationDto.SourceStation} to {searchStationDto.DestinationStation}");
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
