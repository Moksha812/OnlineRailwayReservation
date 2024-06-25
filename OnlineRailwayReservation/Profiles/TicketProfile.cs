using AutoMapper;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Profiles
{
    public class TicketProfile: Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, BookTicketDto>().ReverseMap();
        }
    }
}
