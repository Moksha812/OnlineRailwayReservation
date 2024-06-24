using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class StationDto
    {
       public int StationId {  get; set; }
        public string StationName { get; set; }
        public string city { get; set; }
        
        public string State { get; set; }
    }
}
