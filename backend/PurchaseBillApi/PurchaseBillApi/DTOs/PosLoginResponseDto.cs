namespace PurchaseBillApi.DTOs
{
    public class PosLoginResponseDto
    {
        public int Status_Code { get; set; }
        public string Sync_Time { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<PosResponseBodyDto> Response_Body { get; set; } = new();
    }
}
