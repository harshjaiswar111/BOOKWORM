using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Services.Implementation;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;

            _logger.LogInformation("============AuthController hit============");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {

            _logger.LogInformation("Login action started for username/email: {Email}", request.Email);
            try
            {
                var result = await _authService.LoginAsync(request);

                _logger.LogInformation("Login successful for username/email: {Email}", request.Email);

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Login failed for username/email: {Email}", request.Email);
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            await _authService.RegisterAsync(request);
            return Ok("User registered successfully");
        }


        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDto request)
        {
            await _authService.RegisterAdminAsync(request);
            return Ok("Admin registered successfully");
        }

    }

}
