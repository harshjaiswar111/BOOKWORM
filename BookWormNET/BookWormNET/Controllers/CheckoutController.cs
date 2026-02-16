using BookWormNET.dto;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;              // 🔥 ADD THIS

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequestDto request)
        {
            var userIdClaim =
                User.FindFirst("UserId")?.Value ??
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            await _checkoutService.CheckoutAsync(userId, request);

            return Ok();
        }
    }
}
