using BookWormNET.Data;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "ROLE_USER, ROLE_ADMIN")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(User.FindFirst("UserId")!.Value);
            var result = await _userService.GetProfileAsync(userId);
            return Ok(result);
        }
    }

}
