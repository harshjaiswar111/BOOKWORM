using BookWormNET.dto;
using System.Threading.Tasks;

namespace BookWormNET.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task CheckoutAsync(int userId, CheckoutRequestDto request);
    }
}
