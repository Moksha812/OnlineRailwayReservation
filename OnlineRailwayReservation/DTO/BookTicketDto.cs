namespace OnlineRailwayReservation.DTO
{
    public class BookTicketDto
    {
        public List<PassengerDto> Passengers { get; set; }
        public string SourceStation { get; set; }
        public string DestinationStation { get; set; }
        public string TrainName { get; set; }
        public DateTime TravelDate { get; set; }
    }

    public class PassengerDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Quota { get; set; }
    }
}
