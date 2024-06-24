using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.Models
{
    public class Station
    {
        [Key]
        public int StationId { get; set; }
        [Required]
        [MaxLength(100)]
        public string StationName { get; set; }
        [Required]
        [MaxLength(100)]
        public string city {  get; set; }
        [Required]
        [MaxLength(100)]
        public string State {  get; set; }
        public ICollection<Train> SourceTrains { get; set; }
        public ICollection<Train> DestinationTrains { get; set; }
    }
}
