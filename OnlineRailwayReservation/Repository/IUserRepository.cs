using OnlineRailwayReservation.DTO;
using OnlineRailwayReservation.Models;

namespace OnlineRailwayReservation.Repository
{
    public interface IUserRepository
    {
        public Task<User> CreateUser(UserDto userDto);
        public Task<User> LoginUser(LoginDto loginDto);
    }
}
