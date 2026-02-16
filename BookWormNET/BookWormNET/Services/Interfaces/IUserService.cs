using BookWormNET.Models;

namespace BookWormNET.Services.Interfaces
{
    public interface IUserService
    {
        Task<object> GetProfileAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        void CreateUser(User user);
    }
}
