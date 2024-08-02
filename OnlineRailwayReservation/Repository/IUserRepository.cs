using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(UserDto userDto);
        public Task<LoginResponseDto?> LoginUser(LoginDto loginDto);
        public Task<bool> CheckUserExists(string email);
    }
}
