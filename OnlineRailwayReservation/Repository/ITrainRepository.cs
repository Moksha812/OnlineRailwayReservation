using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface ITrainRepository
    {
        Task<IEnumerable<Train>> GetAllTrains();
        Task<Train> GetTrainById(int id);
        Task<Train> AddTrain(Train train);
        Task<Train> UpdateTrain(Train train);
        Task DeleteTrain(int trainId);
    }
}
