using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet("Exists/{email}")]
        public async Task<ActionResult<bool>> CheckUserExists(string email)
        {
            var exists = await _userRepository.CheckUserExists(email);
            return Ok(exists);
        }
        [HttpPost("Register")]
        public async Task<ActionResult> CreateUser(UserDto userDto)
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
        public async Task<ActionResult> LoginUser(LoginDto loginDto)
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
