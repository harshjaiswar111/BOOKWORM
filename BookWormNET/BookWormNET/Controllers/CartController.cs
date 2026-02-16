using BookWormNET.dto;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookWormNET.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCart dto)
        {
            //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (userIdClaim == null)
            //{
            //    return Unauthorized("UserId not found in token");
            //}

            //int userId = int.Parse(userIdClaim);

            var cartItem = await _cartService.AddToCartAsync(
            dto.UserId,
            dto.ProductId,
            dto.Qty
    );

            return Ok(cartItem);
        }


        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCartByUserId(int userId)
        {
            var userIdClaim =
                User.FindFirst("UserId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User not logged in");

            int loggedInUserId = int.Parse(userIdClaim);

            // 🔐 security check (VERY IMPORTANT)
            if (loggedInUserId != userId)
                return Forbid("You cannot access another user's cart");

            var cartItems = await _cartService.GetcartByUserAsync(userId);
            return Ok(cartItems);
        }


        [HttpDelete("remove/{cartId:int}")]
        public async Task<IActionResult> DeleteCartItem(int cartId)
        {
            var success = await _cartService.DeleteCartItemAsync(cartId);

            if (!success)
                return NotFound("Cart item not found");

            return NoContent();
        }

    }
}
