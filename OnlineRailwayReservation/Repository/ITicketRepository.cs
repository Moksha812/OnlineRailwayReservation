using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface ITicketRepository
    {
        public Task<Ticket> BookTicket(BookTicketDto bookTicketDto);
        public Task<bool> CancelTicket(int pnrNumber);
        public Task<IEnumerable<TicketResponseDto>> GetTicketsByUserId(int userId);
    }
}
