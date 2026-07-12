using PurchaseBillApi.DTOs;
using System.Text.Json;
using System.Net.Http.Headers;

namespace PurchaseBillApi.Services
{
    public class PosApiService : IPosApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PosApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<LoginResultDto> LoginAsync(LoginRequestDto request)
        {
            var baseUrl = _configuration["PosApi:BaseUrl"];
            var deviceId = _configuration["PosApi:DeviceId"];

            var posRequest = new PosLoginRequestDto
            {
                Device_Id = deviceId ?? string.Empty,
                Company_Code = request.Email,
                API_Body = new PosApiBodyDto
                {
                    Username = request.Email,
                    Pw = request.Password
                }
            };

            // NOTE: We build the request content manually (instead of PostAsJsonAsync)
            // because this external API rejects requests where Content-Type includes
            // "charset=utf-8" (which PostAsJsonAsync adds by default), returning a
            // generic "Invalid Login Details" instead of a parsing error.
            var json = JsonSerializer.Serialize(posRequest);
            var httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(baseUrl, httpContent);
            }
            catch (HttpRequestException)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Unable to reach the login service. Please try again later."
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = $"Login service returned an unexpected error ({(int)response.StatusCode})."
                };
            }

            var posResponse = await response.Content.ReadFromJsonAsync<PosLoginResponseDto>();
            var body = posResponse?.Response_Body?.FirstOrDefault();

            bool loginSucceeded = body?.User_Locations != null && body.User_Locations.Count > 0;

            if (!loginSucceeded)
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = body?.Doc_Msg ?? "Invalid email or password."
                };
            }

            return new LoginResultDto
            {
                Success = true,
                Message = "Login successful.",
                Locations = body!.User_Locations
            };
        }
    }
}