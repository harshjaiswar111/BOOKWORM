using BookWormNET.dto;

namespace BookWormNET.Services.Interfaces
{
    public interface IOrderHistoryService
    {
        Task<List<OrderHistoryDto>> GetOrderHistoryAsync(int userId);
        Task<List<OrderHistoryDto>> GetAllOrderHistoryAsync();
    }
}