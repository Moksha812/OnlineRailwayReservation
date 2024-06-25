using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public class TrainRepository : ITrainRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public TrainRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        public async Task<Train> AddTrain(Train train)
        {
            try
            {
                context.Trains.Add(train);
                await context.SaveChangesAsync();
                return train;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add train");
            }
        }

        public async Task DeleteTrain(int trainId)
        {
            try
            {
                var train = await context.Trains.FindAsync(trainId);
                if (train != null)
                {
                    context.Trains.Remove(train);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not delete train");
            }
        }

        public async Task<IEnumerable<Train>> GetAllTrains()
        {
            try
            {
                return await context.Trains.Include(t => t.SourceStation)
                    .Include(t => t.DestinationStation)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("could not retrieve trains");
            }
        }

        public async Task<Train> GetTrainById(int id)
        {
                try
                {
                    return await context.Trains.Include(t => t.SourceStation)
                        .Include(t => t.DestinationStation)
                        .FirstOrDefaultAsync(t => t.Train_Id == id);
                }
            catch (Exception ex)
            {
                throw new Exception("could not retrieve trains");
            }
        }

        public async Task<Train> UpdateTrain(int id, TrainDto trainDTO)
        {
            try
            {
                var train = await context.Trains.FirstOrDefaultAsync(x => x.Train_Id == id);
                if (train == null) return null;

                _mapper.Map(trainDTO, train);
                //train.Train_Id = id;

                context.Trains.Update(train);
                await context.SaveChangesAsync();
                return train;
            }
            catch (Exception ex)
            {
                throw new Exception("could not update train");
            }
        }

        public async Task<IEnumerable<Train>> GetTrainsBySourceAndDestinationStations(string sourceStation, string destinationStation)
        {
            try
            {
                var trains = await context.Trains.Where(x => x.SourceStation.StationName.ToLower().Equals(sourceStation.ToLower()) 
                                && x.DestinationStation.StationName.ToLower().Equals(destinationStation.ToLower())).ToListAsync();
                return trains.IsNullOrEmpty() ? null : trains;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
