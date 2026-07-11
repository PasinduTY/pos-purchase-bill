namespace PurchaseBillApi.DTOs
{
    public class PosLoginRequestDto
    {
        public string API_Action { get; set; } = "GetLoginData";
        public string Device_Id { get; set; } = string.Empty;
        public string Sync_Time { get; set; } = "";
        public string Company_Code { get; set; } = string.Empty;
        public PosApiBodyDto API_Body { get; set; } = new();
    }
}
