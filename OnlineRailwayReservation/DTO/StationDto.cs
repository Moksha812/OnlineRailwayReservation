using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class StationDto
    {
       public int StationId {  get; set; }
        [Required]
        public string StationName { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string State { get; set; }
    }
}
