using PurchaseBillApi.DTOs;

namespace PurchaseBillApi.Services
{
    public interface IPosApiService
    {
        Task<LoginResultDto> LoginAsync(LoginRequestDto request);
    }
}
