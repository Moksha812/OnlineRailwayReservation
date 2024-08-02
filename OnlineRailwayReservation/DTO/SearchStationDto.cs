using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class SearchStationDto
    {
        [Required]
        public string? SourceStation { get; set; }
        [Required]
        public string? DestinationStation { get; set; }
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }
    }
}
