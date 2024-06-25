using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineRailwayReservation.Models
{
    public class Passenger
    {
        [Key]
        public int Passenger_Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Passenger_Name { get; set; }
        [Required]
        [MaxLength(20)]
        public string Gender {  get; set; }
        [Required]
        public string Quota {  get; set; }
        [Required]
        public int Age {  get; set; }
        [Required]
        public int Seat_No {  get; set; }
        [Required]
        [JsonIgnore]
        public int PnrNumber {  get; set; }
        [ForeignKey("PnrNumber")]
        [JsonIgnore]
        public Ticket Ticket {  get; set; }
    }
}
