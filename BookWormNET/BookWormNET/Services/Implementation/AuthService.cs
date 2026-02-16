using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly BookwormDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthService(BookwormDbContext context, JwtService jwtService, IConfiguration config)
        {
            _context = context;
            _jwtService = jwtService;
            _config = config;
        }

        public async Task<object> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == request.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(request.Password, user.UserPassword))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _jwtService.GenerateToken(user);

            return new
            {   
                token,
                userId = user.UserId,
                role = user.IsAdmin == 1 ? "ROLE_ADMIN" : "ROLE_USER"
            };
        }

        public async Task RegisterAsync(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.UserEmail == request.UserEmail))
                throw new Exception("Email already exists");

            var user = new User
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                UserPhone = request.UserPhone,
                UserAddress = request.UserAddress,
                IsAdmin = 0,
                JoinDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }


        public async Task RegisterAdminAsync(AdminRegisterDto request)
        {
            string secret =
                _config["AdminSettings:AdminSecretKey"];

            if (request.AdminSecretKey != secret)
                throw new UnauthorizedAccessException("Invalid admin secret key");

            if (await _context.Users.AnyAsync(u => u.UserEmail == request.UserEmail))
                throw new Exception("User already exists");

            var admin = new User
            {
                UserName = request.UserName,
                UserEmail = request.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsAdmin = 1,
                JoinDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();
        }



    }

}
