using Microsoft.EntityFrameworkCore;
using PurchaseBillApi.Data;
using PurchaseBillApi.DTOs;
using PurchaseBillApi.Models;

namespace PurchaseBillApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPosApiService _posApiService;
        private readonly AppDbContext _dbContext;
        private readonly ITokenService _tokenService;

        public AuthService(IPosApiService posApiService, AppDbContext dbContext, ITokenService tokenService)
        {
            _posApiService = posApiService;
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto request)
        {
            var result = await _posApiService.LoginAsync(request);

            if (!result.Success || result.Locations == null)
            {
                return result;
            }

            await SaveLocationsAsync(result.Locations);

            result.Token = _tokenService.GenerateToken(request.Email);

            return result;
        }

        private async Task SaveLocationsAsync(List<UserLocationDto> locations)
        {
            var incomingCodes = locations.Select(l => l.Location_Code).ToList();

            var existingLocations = await _dbContext.Location_Details
                .Where(l => incomingCodes.Contains(l.Location_Code))
                .ToListAsync();

            foreach (var location in locations)
            {
                var existing = existingLocations
                    .FirstOrDefault(e => e.Location_Code == location.Location_Code);

                if (existing != null)
                {
                    existing.Location_Name = location.Location_Name;
                }
                else
                {
                    _dbContext.Location_Details.Add(new LocationDetail
                    {
                        Location_Code = location.Location_Code,
                        Location_Name = location.Location_Name
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
