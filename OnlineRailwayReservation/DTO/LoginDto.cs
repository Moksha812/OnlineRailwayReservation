using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineRailwayReservation.DTO
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public string token { get; set; }
        public string email { get; set; }
        public int userId { get; set; }
        public string userRole { get; set; }
    }
}
