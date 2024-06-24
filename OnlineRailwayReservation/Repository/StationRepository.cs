using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public class StationRepository : IStationRepository
    {
        private readonly ApplicationDbContext context;
        public StationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddStationAsync(Station station)
        {
            try
            {
                await context.Stations.AddAsync(station);
                await context.SaveChangesAsync();
            }
            
            catch (Exception ex) {
                throw new Exception("Could not Add stations");
            }
        }

        public async Task DeleteStationAsync(int id)
        {
            var station=await context.Stations.FirstOrDefaultAsync(x=>x.StationId==id);
            if (station != null)
            {
                context.Stations.Remove(station);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Station>> GetAllStationsAsync()
        {
            try
            {
                return await context.Stations.ToListAsync();
            }
            catch (Exception ex) {
                throw new Exception("Could not retrieve stations");
            }
        }

        public async Task<Station> GetStationByIdAsync(int id)
        {
            try
            {
                return await context.Stations.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve stations");
            }
        }

        public async Task UpdateStationAsync(Station station)
        {
            try
            {
                context.Stations.Update(station);
                await context.SaveChangesAsync();
            }
           
            catch (Exception ex) {
                throw new Exception("Could not retrieve stations");
            }
        }
    }
}
