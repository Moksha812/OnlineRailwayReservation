using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineRailwayReservation.Models
{
    public class Ticket
    {
        [Key]
        public int PnrNumber {  get; set; }
        [Required]
        public int Train_Id { get; set; }
        [Required]
        public int User_Id {  get; set; }
        public int No_of_seats {  get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TravelDate { get; set; }
        [Required]
        public Boolean Status { get; set; }
        public decimal Total_fare {  get; set; }
        [ForeignKey("Train_Id")]
        public Train Train { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public ICollection<Passenger> Passengers { get; set; }
    }
}
