using BookWormNET.dto;
using BookWormNET.Services.Implementation;

namespace BookWormNET.Services.Interfaces
{
    public interface ILibraryCheckoutService
    {
        void Checkout(LibraryCheckoutRequest request);

    }
}
