using BookWormNET.Data;
using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookWormNET.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly BookwormDbContext _context;

        public CartService(BookwormDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCartAsync(int userId, int productId, int qty)
        {
            var cartItem = await _context.Carts
               .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
               
                cartItem.Qty += qty;
            }
            else
            {
                
                cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Qty = qty
                };

                _context.Carts.Add(cartItem);
            }


            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> DeleteCartItemAsync(int Cartid)
        {
            var cartItem = await _context.Carts.FindAsync(Cartid);
            if (cartItem == null)
                return false;

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

       

        public async Task<IEnumerable<Cart>> GetcartByUserAsync(int userid)
        {
            return await _context.Carts
                .Where(c => c.UserId == userid)
                .Include(c => c.Product)
                .ToListAsync();
        }
    }
}
