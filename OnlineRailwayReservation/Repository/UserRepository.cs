using AutoMapper;
using BCrypt.Net;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;
using System.IdentityModel.Tokens.Jwt;
//using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace OnlineRailwayReservation.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext context, IMapper mapper,IConfiguration configuration)
        {
            this.context = context;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginUser(LoginDto loginDto)
        {
            try
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
                if (existingUser != null && BCrypt.Net.BCrypt.EnhancedVerify(loginDto.Password, existingUser.Password))
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, existingUser.User_Id.ToString()),
                            new Claim(ClaimTypes.Email, existingUser.Email),
                            new Claim(ClaimTypes.Role, existingUser.User_Role)
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"]
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                        var genToken = tokenHandler.WriteToken(token);
                    var res = new LoginResponseDto
                    {
                        token = genToken,
                        userId = existingUser.User_Id,
                        email = existingUser.Email,
                        userRole = existingUser.User_Role,
                    };
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public async Task<bool> CheckUserExists(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user != null;
        }
        public async Task<User> CreateUser(UserDto userDto)
        {
            try
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);
                if (existingUser == null) 
                {
                    var newUser = new User
                    {
                        User_Name = userDto.Name,
                        Email = userDto.Email,
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.Password),
                        PhoneNumber = userDto.PhoneNumber,
                        User_Role = userDto.UserRole
                    };

                    context.Users.Add(newUser);
                    await context.SaveChangesAsync();
                    var subject = "Welcome to Our Online Reservation system";
                    var body = $"<h1>Welcome, {newUser.User_Name}!</h1><p>Thank you for registering.</p>";
                    await SendEmailAsync(newUser.Email, subject, body);
                    return newUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["Smtp:SenderName"], _configuration["Smtp:SenderEmail"]));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["Smtp:Server"], int.Parse(_configuration["Smtp:Port"]), false);
                client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }
    }
}
