using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.DTO
{
    public class TicketResponseDto
    {
        public int PnrNumber { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime BookinigDate { get; set; }
        public DateTime TravelDate { get; set; }
        public bool Status { get; set; }
        public decimal TotalFare { get; set; }
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public DateTime SourceStationArrivalTime { get; set; }
        public DateTime DestinationStationArrivalTime { get; set; }
        public ICollection<Passenger> Passengers { get; set; }
    }
}
