namespace PurchaseBillApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string email);
    }
}
