using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using OnlineRailwayReservation.Data;
using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        public async Task<User> LoginUser(LoginDto loginDto)
        {
            try
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
                if (existingUser != null && BCrypt.Net.BCrypt.EnhancedVerify(loginDto.Password, existingUser.Password))
                    return existingUser;
                return null;
            }
            catch (Exception ex)
            {
                throw;

            }
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
                    return newUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

    }
}
