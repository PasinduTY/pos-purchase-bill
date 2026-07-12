using PurchaseBillApi.DTOs;

namespace PurchaseBillApi.Services
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto request);
    }
}
