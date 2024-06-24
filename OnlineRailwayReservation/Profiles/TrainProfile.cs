using AutoMapper;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Profiles
{
    public class TrainProfile:Profile
    {
        public TrainProfile()
        {
            CreateMap<Train,TrainDto>().ReverseMap();
        }
    }
}
