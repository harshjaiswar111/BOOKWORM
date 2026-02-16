    using BookWormNET.Models;

    namespace BookWormNET.Services.Interfaces
    {
        public interface ICartService
        {
            Task<Cart> AddToCartAsync(int Userid, int Productid, int qty);
            Task<bool> DeleteCartItemAsync(int Cartid);
            Task<IEnumerable<Cart>> GetcartByUserAsync(int userid);

        }
    }
