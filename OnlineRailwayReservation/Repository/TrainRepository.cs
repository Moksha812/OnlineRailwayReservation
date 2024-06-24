using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public class TrainRepository : ITrainRepository
    {
        private readonly ApplicationDbContext context;
        public TrainRepository(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task<Train> UpdateTrain(Train train)
        {
            try
            {
                context.Trains.Update(train);
                await context.SaveChangesAsync();
                return train;
            }
            catch (Exception ex)
            {
                throw new Exception("could not update train");
            }
        }
    }
}
