using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;
using System.Net.Http.Headers;

namespace ARIB_Task_MVC.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ARIB_API");
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("Employee/GetAll");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<GetEmployeeWithoutImageResult>() ?? new ApiResponse();

            return await HandleError(response);
        }

        public async Task<ApiResponse> GetListAsync()
        {
            var response = await _httpClient.GetAsync("Employee/GetListEmployees");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<GetListEmployeeResult>() ?? new ApiResponse();

            return await HandleError(response);
        }

        public async Task<GetEmployeeResult> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Employee/Get/{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<GetEmployeeResult>() ?? new GetEmployeeResult { Success = false, ErrorMessage = "Empty response" };

            var error = await response.Content.ReadAsStringAsync();
            return new GetEmployeeResult
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = error
            };
        }

        public async Task<ApiResponse> CreateAsync(EmployeeDto dto)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(dto.FirstName), nameof(dto.FirstName));
            content.Add(new StringContent(dto.LastName), nameof(dto.LastName));
            content.Add(new StringContent(dto.Salary.ToString()), nameof(dto.Salary));
            content.Add(new StringContent(dto.DepartmentId.ToString()), nameof(dto.DepartmentId));
            content.Add(new StringContent(dto.Gender), nameof(dto.Gender));
            if (dto.ManagerId.HasValue)
                content.Add(new StringContent(dto.ManagerId.Value.ToString()), nameof(dto.ManagerId));
            if (dto.ProfileImage != null)
            {
                var fileContent = new StreamContent(dto.ProfileImage.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ProfileImage.ContentType);
                content.Add(fileContent, nameof(dto.ProfileImage), dto.ProfileImage.FileName);
            }

            var response = await _httpClient.PostAsync("Employee/Create", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();

            return await HandleError(response);
        }

        public async Task<ApiResponse> UpdateAsync(int id, EmployeeDto dto)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(dto.FirstName), nameof(dto.FirstName));
            content.Add(new StringContent(dto.LastName), nameof(dto.LastName));
            content.Add(new StringContent(dto.Salary.ToString()), nameof(dto.Salary));
            content.Add(new StringContent(dto.DepartmentId.ToString()), nameof(dto.DepartmentId));
            content.Add(new StringContent(dto.Gender), nameof(dto.Gender));
            if (dto.ManagerId.HasValue)
                content.Add(new StringContent(dto.ManagerId.Value.ToString()), nameof(dto.ManagerId));
            if (dto.ProfileImage != null)
            {
                var fileContent = new StreamContent(dto.ProfileImage.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(dto.ProfileImage.ContentType);
                content.Add(fileContent, nameof(dto.ProfileImage), dto.ProfileImage.FileName);
            }

            var response = await _httpClient.PutAsync($"Employee/Update?id={id}", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();

            return await HandleError(response);
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Employee/Delete?id={id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ApiResponse>() ?? new ApiResponse();

            return await HandleError(response);
        }

        private async Task<ApiResponse> HandleError(HttpResponseMessage response)
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
}
