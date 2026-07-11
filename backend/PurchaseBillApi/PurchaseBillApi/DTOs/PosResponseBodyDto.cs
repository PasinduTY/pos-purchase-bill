namespace PurchaseBillApi.DTOs
{
    public class PosResponseBodyDto
    {
        public string? User_Code { get; set; }
        public string? User_Display_Name { get; set; }
        public string? Email { get; set; }
        public string? Company_Code { get; set; }
        public List<UserLocationDto>? User_Locations { get; set; }
        public string? Doc_Msg { get; set; }
    }
}
