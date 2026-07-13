namespace PurchaseBillApi.DTOs
{
    public class PurchaseBillSubmitRequestDto
    {
        public List<PurchaseBillItemDto> Items { get; set; } = new();
    }
}
