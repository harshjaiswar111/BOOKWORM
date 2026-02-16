using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    //[Authorize(Roles = "ROLE_USER, ROLE_ADMIN")]
    [ApiController]
    [Route("api/orders")]
    public class OrderHistoryController : ControllerBase
    {
        private readonly IOrderHistoryService _orderHistoryService;

        public OrderHistoryController(IOrderHistoryService orderHistoryService)
        {
            _orderHistoryService = orderHistoryService;
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetOrderHistory(int userId)
        {
            var orders = await _orderHistoryService.GetOrderHistoryAsync(userId);
            return Ok(orders);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderHistoryService.GetAllOrderHistoryAsync();
            return Ok(orders);
        }
    }
}