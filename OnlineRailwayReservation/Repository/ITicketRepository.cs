using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface ITicketRepository
    {
        public Task<Ticket> BookTicket(BookTicketDto bookTicketDto,int User_Id);
        public Task<bool> CancelTicket(int pnrNumber);
        public Task<IEnumerable<TicketResponseDto>> GetTicketsByUserId(int userId);
        public Task<TicketResponseDto> GetTicketsByPNR(int pnr);
    }
}
