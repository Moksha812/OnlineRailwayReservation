using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace OnlineRailwayReservation.Models
{
    public class Train
    {
        [Key]
        public int Train_Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Train_Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Train_Number {  get; set; }
        [Required]
        public int Total_seats {  get; set; }
        public int Available_seats { get; set; }
        public int Available_seats_General {  get; set; }
        public int Available_seats_Ladies {  get; set; }
        [Required]
        public int SourceStationId {  get; set; }
        [Required]
        public int DestinationStationId {  get; set; }
        [Required]
        public decimal Fare {  get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }
        [ForeignKey("SourceStationId")]
        public Station SourceStation { get; set; }
        [ForeignKey("DestinationStationId")]
        public Station DestinationStation { get; set; }
        public ICollection<Ticket> Tickets{ get; set; }


    }
}
