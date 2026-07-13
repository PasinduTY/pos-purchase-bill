namespace PurchaseBillApi.DTOs
{
    public class PurchaseBillSubmitResultDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
    }
}
