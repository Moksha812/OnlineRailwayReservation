using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class BookTicketDto
    {
        [Required]
        public List<PassengerDto> Passengers { get; set; }
        [Required]
        [MaxLength(100)]
        public string SourceStation { get; set; }
        [Required]
        [MaxLength(100)]
        public string DestinationStation { get; set; }
        [Required]
        [MaxLength(100)]
        public string TrainName { get; set; }
        [Required]
        public DateTime TravelDate { get; set; }
    }

    public class PassengerDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Quota { get; set; }
    }
}
