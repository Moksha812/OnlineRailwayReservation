using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface IStationRepository
    {
        Task<IEnumerable<Station>> GetAllStationsAsync();
        Task<Station>GetStationByIdAsync(int id);
        Task AddStationAsync(Station station);
        Task UpdateStationAsync(Station station);
        Task DeleteStationAsync(int id);
    }
}
