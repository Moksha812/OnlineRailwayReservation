using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class UserDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string UserRole { get; set; }

    }
}
