namespace PurchaseBillApi.DTOs
{
    public class LoginResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<UserLocationDto>? Locations { get; set; }
        public string? Token { get; set; }
    }
}
