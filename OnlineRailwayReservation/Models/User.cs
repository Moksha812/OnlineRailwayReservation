using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineRailwayReservation.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters")]
        public string User_Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters")]
        [JsonIgnore]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "User Role is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "User Role must be between 3 and 20 characters")]
        public string User_Role { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
