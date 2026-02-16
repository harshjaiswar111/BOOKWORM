using BookWormNET.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookWormNET.Services.Implementation
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            string role = user.IsAdmin == 1 ? "ROLE_ADMIN" : "ROLE_USER";

            var claims = new List<Claim>
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.UserEmail),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, role)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["JwtSettings:DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
