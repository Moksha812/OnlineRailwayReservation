using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class CreateStationDto
    {
        [Required]
        [MaxLength(100)]
        public string StationName { get; set; }
        [Required]
        [MaxLength(100)]
        public string city { get; set; }
        [Required]
        [MaxLength(100)]
        public string State { get; set; }
    }
}
