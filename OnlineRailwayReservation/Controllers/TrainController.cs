using AutoMapper;
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
                var trainDTOs = _mapper.Map<IEnumerable<TrainDto>>(trains);
                return Ok(trainDTOs);
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
                var trainDTO = _mapper.Map<TrainDto>(train);
                return Ok(trainDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TrainDto>> AddTrain(TrainDto trainDTO)
        {
            try
            {
                var train = _mapper.Map<Train>(trainDTO);
                train = await _trainRepository.AddTrain(train);
                trainDTO.Train_Id = train.Train_Id;
                return CreatedAtAction(nameof(GetTrainById), new { id = trainDTO.Train_Id }, trainDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrain(int id, TrainDto trainDTO)
        {
            if (id != trainDTO.Train_Id)
            {
                return BadRequest();
            }
            var train = _mapper.Map<Train>(trainDTO);
            try
            {
                await _trainRepository.UpdateTrain(train);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return NoContent();
        }

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
    }
}
