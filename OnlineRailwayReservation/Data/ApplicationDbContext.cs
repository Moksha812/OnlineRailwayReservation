using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Models;
namespace OnlineRailwayReservation.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Passenger> Passengers {  get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Train>()
                .HasOne(t=>t.SourceStation)
                .WithMany(s=>s.SourceTrains)
                .HasForeignKey(t=>t.SourceStationId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Train>()
                .HasOne(t => t.DestinationStation)
                .WithMany(s => s.DestinationTrains)
                .HasForeignKey(t => t.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }

    }
}
