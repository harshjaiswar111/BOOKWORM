using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly BookwormDbContext _context;

        public UserService(BookwormDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetProfileAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null) throw new Exception("User not found");

            return new
            {
                user.UserName,
                user.UserEmail,
                user.UserPhone,
                user.UserAddress
            };
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email);
        }
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }

}
