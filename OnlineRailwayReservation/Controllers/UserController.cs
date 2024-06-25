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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<TrainDto>> CreateUser(UserDto userDto)
        {
            try
            {
                var result = await _userRepository.CreateUser(userDto);
                if (result == null) return BadRequest(new { Message = $"User with email: {userDto.Email} already exists." });
                return CreatedAtAction(nameof(CreateUser), new { id = result.User_Id }, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<TrainDto>> LoginUser(LoginDto loginDto)
        {
            try
            {
                var result = await _userRepository.LoginUser(loginDto);
                if (result == null) return BadRequest(new { Message = $"Incorrect email or password" });
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
