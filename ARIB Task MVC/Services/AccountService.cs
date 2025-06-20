using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;

namespace ARIB_Task_MVC.Services
{
    public class AccountService:IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ARIB_API");
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/Login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
                return result ?? new LoginResultDto { Success = false, ErrorMessage = "Empty response" };
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                return new LoginResultDto
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = errorMessage
                };
            }
        }
        public async Task<ApiResponse> RegisterEmployeeAsync(RegisterationEmployeeDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/RegisterEmployee", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error
                };
            }
        }
        public async Task<ApiResponse> RegisterAdminAsync(RegisterationAdminDto model)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/RegisterAdmin", model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error
                };
            }
        }
        public async Task<ApiResponse> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var url = $"Account/ChangePassword?email={email}&currentPassword={currentPassword}&newPassword={newPassword}";
            var response = await _httpClient.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error
                };
            }
        }
        public async Task<ApiResponse> ChangeStatusOfAccountAsync(string email, bool active)
        {
            var url = $"Account/ChangeStatusOfAccount?email={email}&active={active}";
            var response = await _httpClient.PutAsync(url, null);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new ApiResponse
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error
                };
            }
        }
        public async Task<BasicUserResult> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("Account/GetAllUser");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BasicUserResult>() ?? new BasicUserResult();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BasicUserResult
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error,
                    Data=response.Content.ReadAsStringAsync().Result
                };
            }
        }
    }
}
