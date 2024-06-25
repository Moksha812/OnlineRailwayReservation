using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Repository;

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
        public async Task<IActionResult> BookTicket(BookTicketDto bookTicketDto)
        {
            if (bookTicketDto.Passengers.Count() > 6) return BadRequest("Cannot Book tickets for more than 6 people at a time");
            try
            {
                var result = await _ticketRepository.BookTicket(bookTicketDto);
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

        [HttpPut("CancelTicket/{pnrNumber}")]
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
