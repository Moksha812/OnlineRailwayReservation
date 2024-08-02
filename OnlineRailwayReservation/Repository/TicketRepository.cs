using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;
//using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;

namespace OnlineRailwayReservation.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public TicketRepository(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this._mapper = mapper;
            this._configuration = configuration;
        }
        public async Task<Ticket> BookTicket(BookTicketDto bookTicketDto, int user_Id)
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

                if (sourceStation == null || destinationStation == null || sourceStation == destinationStation || train == null || bookTicketDto.TravelDate < DateTime.Now) return null;

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
                ticket.User = await context.Users.FirstOrDefaultAsync(x => x.User_Id == user_Id);

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
                var user = await context.Users.FirstOrDefaultAsync(x => x.User_Id == user_Id);
                var subject = "Ticket Details For Your Next Trip";
                var body = $"<h1>Ticket Details</h1><p>PNR Number: {ticket.PnrNumber}</p><p>Train: {ticket.Train.Train_Name}</p><p>Travel Date: {ticket.TravelDate}</p><p>Total Fare: {ticket.Total_fare}</p>";
                body += "<h2>Passengers:</h2><ul>";
                foreach (var passenger in ticket.Passengers)
                {
                    body += $"<li>Name: {passenger.Passenger_Name}, Age: {passenger.Age}, Gender: {passenger.Gender}, Quota: {passenger.Quota}, Seat No: {passenger.Seat_No}</li>";
                }
                body += "</ul>";
                if (user != null)
                {
                    await SendEmailAsync(user.Email, subject, body);
                }
                return ticket;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["Smtp:SenderName"], _configuration["Smtp:SenderEmail"]));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["Smtp:Server"], int.Parse(_configuration["Smtp:Port"]), false);
                client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
        //private async Task SendTicketEmail(string email, Ticket ticket)
        //{
        //    var smtpClient = new SmtpClient("smtp.gmail.com")
        //    {
        //        Port = 587,
        //        Credentials = new NetworkCredential("jainmoksha1812@gmail.com", "xwselpteytydqbxh"),
        //        EnableSsl = true,
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("jainmoksha1812@gmail.com"),
        //        Subject = "Your Ticket Booking Details",
        //        Body = $"<h1>Ticket Details</h1><p>PNR Number: {ticket.PnrNumber}</p><p>Train: {ticket.Train.Train_Name}</p><p>Travel Date: {ticket.TravelDate}</p><p>Total Fare: {ticket.Total_fare}</p>",
        //        IsBodyHtml = true,
        //    };
        //    mailMessage.To.Add(email);

        //    await smtpClient.SendMailAsync(mailMessage);
        //}

        public async Task<bool> CancelTicket(int pnrNumber)
        {
            try
            {
                var ticket = await context.Tickets
                                .Include(t => t.Train)
                                .Include(t => t.Passengers)
                                .SingleOrDefaultAsync(x => x.PnrNumber == pnrNumber);

                //var ticket = await context.Tickets.SingleOrDefaultAsync(x => x.PnrNumber == pnrNumber);

                if (ticket == null) return false;

                ticket.Status = false;
                var train = ticket.Train;
                train.Available_seats += ticket.No_of_seats;

                foreach (var passenger in ticket.Passengers)
                {
                    if (passenger.Quota.ToLower() == "general")
                        train.Available_seats_General++;
                    else if (passenger.Quota.ToLower() == "ladies")
                        train.Available_seats_Ladies++;
                }
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
        public async Task<TicketResponseDto> GetTicketsByPNR(int pnr)
        {
            try
            {
                var ticket = await context.Tickets.Include(t => t.User)
                                .Include(t => t.Train)
                                .Include(t => t.Train.SourceStation)
                                .Include(t => t.Train.DestinationStation)
                                .Include(t => t.Passengers).FirstOrDefaultAsync(x => x.PnrNumber == pnr);
                var res = new TicketResponseDto
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
                };
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
