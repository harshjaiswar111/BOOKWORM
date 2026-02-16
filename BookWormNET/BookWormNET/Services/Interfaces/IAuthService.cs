using BookWormNET.dto;

namespace BookWormNET.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginRequestDto request);
        Task RegisterAsync(RegisterDto request);
        Task RegisterAdminAsync(AdminRegisterDto request);
        
    }
}
