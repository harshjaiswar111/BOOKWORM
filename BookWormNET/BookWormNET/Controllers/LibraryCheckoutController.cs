using BookWormNET.dto;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace BookWormNET.Controllers
{

    [ApiController]
    [Route("api/library")]
    public class LibraryCheckoutController : ControllerBase
    {
        private readonly ILibraryCheckoutService _checkoutService;

        public LibraryCheckoutController(ILibraryCheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] LibraryCheckoutRequest request)
        {
            _checkoutService.Checkout(request);
            return Ok(new { message = "Library checkout successful" });
        }
    }
}
