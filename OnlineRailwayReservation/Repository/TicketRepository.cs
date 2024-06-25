using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public TicketRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }
        public async Task<Ticket> BookTicket(BookTicketDto bookTicketDto)
        {
            try
            {
                var sourceStation = await context.Stations.FirstOrDefaultAsync(x => x.StationName.ToLower().
                                            Equals(bookTicketDto.SourceStation.ToLower()));
                var destinationStation = await context.Stations.FirstOrDefaultAsync(x => x.StationName.ToLower().
                                            Equals(bookTicketDto.DestinationStation.ToLower()));

                var train = await context.Trains.FirstOrDefaultAsync(x => x.Train_Name.ToLower().
                                            Equals(bookTicketDto.TrainName.ToLower()));
                var totalPassenngers = bookTicketDto.Passengers.Count();

                if (sourceStation == null || destinationStation == null || train == null || bookTicketDto.TravelDate < DateTime.Now) return null;

                int gnSeats = 0, ladiesSeats = 0;
                bookTicketDto.Passengers.ForEach(x =>
                {
                    if (x.Quota.ToLower().Equals("general")) gnSeats++;
                    else if (x.Quota.Equals("ladies")) ladiesSeats++;
                });

                if (train.Available_seats < totalPassenngers
                    || train.Available_seats_General < gnSeats
                        || train.Available_seats_Ladies < ladiesSeats)
                {
                    Console.WriteLine("Tickets not available");
                    return null;
                }

                // create a ticket
                var ticket = new Ticket();

                ticket.Total_fare = train.Fare * totalPassenngers;
                ticket.No_of_seats = totalPassenngers;
                ticket.TravelDate = bookTicketDto.TravelDate;
                ticket.BookingDate = DateTime.Now;
                ticket.Train = train;
                ticket.Status = true;
                ticket.Passengers = new List<Passenger>();
                ticket.User = await context.Users.FirstOrDefaultAsync(x => x.User_Id == 1);

                foreach (var passenger in bookTicketDto.Passengers)
                {
                    ticket.Passengers.Add(new Passenger
                    {
                        Age = passenger.Age,
                        Gender = passenger.Gender,
                        Passenger_Name = passenger.Name,
                        Seat_No = train.Available_seats--,
                        Quota = passenger.Quota,
                    });
                }

                train.Available_seats_General -= gnSeats;
                train.Available_seats_Ladies -= ladiesSeats;

                await context.Tickets.AddAsync(ticket);
                await context.SaveChangesAsync();

                return ticket;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CancelTicket(int pnrNumber)
        {
            try
            {
                var ticket = await context.Tickets.SingleOrDefaultAsync(x => x.PnrNumber == pnrNumber);

                if (ticket == null) return false;

                ticket.Status = false;

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TicketResponseDto>> GetTicketsByUserId(int userId)
        {
            try
            {
                var tickets = await context.Tickets.Include(t => t.User)
                                .Include(t => t.Train)
                                .Include(t => t.Train.SourceStation)
                                .Include(t => t.Train.DestinationStation)
                                .Include(t => t.Passengers)
                                .Where(x => x.User_Id == userId).ToListAsync();
                var ticketsRespone = new List<TicketResponseDto>();
                foreach (var ticket in tickets) 
                {
                    ticketsRespone.Add(new TicketResponseDto
                    {
                        BookinigDate = ticket.BookingDate,
                        SourceStation = ticket.Train.SourceStation.StationName,
                        SourceStationArrivalTime = ticket.Train.ArrivalTime,
                        DestinationStation = ticket.Train.DestinationStation.StationName,
                        DestinationStationArrivalTime = ticket.Train.DepartureTime,
                        NumberOfSeats = ticket.No_of_seats,
                        PnrNumber = ticket.PnrNumber,
                        Status = ticket.Status,
                        TotalFare = ticket.Total_fare,
                        TravelDate = ticket.TravelDate,
                        Passengers = ticket.Passengers
                    });
                }
                return ticketsRespone;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
