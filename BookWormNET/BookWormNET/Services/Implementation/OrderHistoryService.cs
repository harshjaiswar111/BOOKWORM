using BookWormNET.Data;
using BookWormNET.dto;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly BookwormDbContext _context;

        public OrderHistoryService(BookwormDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderHistoryDto>> GetAllOrderHistoryAsync()
        {
            return await _context.TransactionItems
                .Include(ti => ti.Transaction)
                .Include(ti => ti.Product)
                .Where(ti =>
                    ti.Transaction != null &&
                    ti.Product != null
                )
                .OrderByDescending(ti => ti.Transaction!.CreatedAt ?? DateTime.MinValue)
                .Select(ti => new OrderHistoryDto
                {
                    TransactionId = ti.Transaction!.TransactionId,
                    ProductName = ti.Product!.ProductName,
                    Amount = (ti.Price ?? 0m) * (ti.Quantity ?? 0),
                    OrderDate = ti.Transaction!.CreatedAt ?? DateTime.MinValue
                })
                .ToListAsync();
        }

        public async Task<List<OrderHistoryDto>> GetOrderHistoryAsync(int userId)
        {
            return await _context.TransactionItems
                .Include(ti => ti.Transaction)
                .Include(ti => ti.Product)
                .Where(ti =>
                    ti.Transaction != null &&
                    ti.Product != null &&
                    ti.Transaction.UserId != null &&
                    ti.Transaction.UserId == userId
                )
                .OrderByDescending(ti => ti.Transaction!.CreatedAt ?? DateTime.MinValue)
                .Select(ti => new OrderHistoryDto
                {
                    TransactionId = ti.Transaction!.TransactionId,
                    ProductName = ti.Product!.ProductName,
                    Amount = (ti.Price ?? 0m) * (ti.Quantity ?? 0),
                    OrderDate = ti.Transaction!.CreatedAt ?? DateTime.MinValue
                })
                .ToListAsync();
        }
    }
}