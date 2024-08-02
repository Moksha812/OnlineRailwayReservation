using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface ITrainRepository
    {
        Task<IEnumerable<Train>> GetAllTrains();
        Task<Train> GetTrainById(int id);
        Task<Train> AddTrain(Train train);
        Task<Train> UpdateTrain(int id, TrainDto trainDTO);
        Task DeleteTrain(int trainId);
        Task<IEnumerable<Train>> GetTrainsBySourceAndDestinationStations(string sourceStation, string destinationStation, DateTime travelDate);
    }
}
