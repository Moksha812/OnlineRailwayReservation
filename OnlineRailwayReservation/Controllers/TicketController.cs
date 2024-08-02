using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Repository;
using System.Security.Claims;

namespace OnlineRailwayReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        [HttpPost("BookTicket")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> BookTicket(BookTicketDto bookTicketDto)
        {
            if (bookTicketDto.Passengers.Count() > 6) return BadRequest("Cannot Book tickets for more than 6 people at a time");
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var result = await _ticketRepository.BookTicket(bookTicketDto,userId);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("Input not correct");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTicketsByUserId/{userId}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> GetTicketsByUserId(int userId)
        {
            try
            {
                var result = await _ticketRepository.GetTicketsByUserId(userId);
                if (!result.IsNullOrEmpty())
                    return Ok(result);
                return BadRequest(new { Message = $"Cannot find tickets for user with id: {userId}" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTicketsByPNR/{pnr}")]
        //[Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> GetTicketsByPNR(int pnr)
        {
            try
            {
                var result = await _ticketRepository.GetTicketsByPNR(pnr);
                if (result!=null)
                    return Ok(result);
                return BadRequest(new { Message = $"Cannot find tickets for user with PNR: {pnr}" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelTicket/{pnrNumber}")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<IActionResult> CancelTicket(int pnrNumber)
        {
            try
            {
                var result = await _ticketRepository.CancelTicket(pnrNumber);
                if (result)
                    return Ok(new { Message = "Ticket cancelled successfully" });
                return BadRequest(new { Message = $"Cannot find ticket with PNR: {pnrNumber}" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
