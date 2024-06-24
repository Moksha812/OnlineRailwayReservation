using AutoMapper;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Profiles
{
    public class StationProfile:Profile
    {
        public StationProfile() {
            CreateMap<Station, StationDto>();
            CreateMap<CreateStationDto, Station>();
            CreateMap<UpdateStationDto, Station>();
        }
    }
}
