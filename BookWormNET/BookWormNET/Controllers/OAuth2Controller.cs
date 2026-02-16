using BookWormNET.Models;
using BookWormNET.Services.Implementation;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookWormNET.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("oauth2")]
    public class OAuth2Controller : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserService _userService;
        private readonly ILogger<OAuth2Controller> _logger;

        public OAuth2Controller(
            JwtService jwtService,
            IUserService userService,
            ILogger<OAuth2Controller> logger)
        {
            _jwtService = jwtService;
            _userService = userService;
            _logger = logger;
        }

        // 🔑 ENTRY POINT (what frontend hits)
        [HttpGet("authorization/google")]
        public IActionResult GoogleLogin()
        {
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = "/oauth2/user"
                },
                "Google"
            );
        }

        // ✅ POST-LOGIN HANDLER (WHAT YOU WANTED)
        [HttpGet("user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOAuth2User()
        {
            // 🔥 EXPLICITLY read Google cookie principal
            var authResult = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (!authResult.Succeeded || authResult.Principal == null)
            {
                _logger.LogError("Google OAuth: Cookie authentication failed");
                return Redirect("http://localhost:5173/admin/login?error=oauth");
            }

            var email =
                authResult.Principal.FindFirst(ClaimTypes.Email)?.Value
                ?? authResult.Principal.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Google OAuth: Email claim missing");
                return Redirect("http://localhost:5173/admin/login?error=oauth");
            }

            // 🔹 DB lookup
            var dbUser = await _userService.GetUserByEmailAsync(email);

            if (dbUser == null)
            {
                dbUser = new User
                {
                    UserEmail = email,
                    UserName = email.Split('@')[0],
                    UserPassword = "Google",
                    IsAdmin = 0,
                    JoinDate = DateOnly.FromDateTime(DateTime.Now)
                };

                _userService.CreateUser(dbUser);
            }

            // 🔐 JWT (UNCHANGED)
            var token = _jwtService.GenerateToken(dbUser);

            // ✅ Redirect to YOUR frontend page
            return Redirect(
                $"http://localhost:5173/login-success?token={token}&userId={dbUser.UserId}"
            );
        }
    }
}
