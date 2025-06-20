using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;

namespace ARIB_Task_MVC.Services
{
    public class DepartmentService:IDepartmentService
    {
        private readonly HttpClient _httpClient;

        public DepartmentService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ARIB_API");
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("Department/GetAll");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GetDepartmentResult>() ?? new ApiResponse();
            }

            var error = await response.Content.ReadAsStringAsync();
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }

        public async Task<ApiResponse> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Department/Get/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GetDepartmentResult>() ?? new ApiResponse();
            }

            var error = await response.Content.ReadAsStringAsync();
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }

        public async Task<ApiResponse> CreateAsync(DepartmentDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Department/Create", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }

            var error = await response.Content.ReadAsStringAsync();
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }

        public async Task<ApiResponse> UpdateAsync(int id, DepartmentDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Department/Update/{id}", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }

            var error = await response.Content.ReadAsStringAsync();
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Department/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();
            }

            var error = await response.Content.ReadAsStringAsync();
            return new ApiResponse
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }
    }
}