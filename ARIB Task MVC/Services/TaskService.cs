using ARIB_Task_MVC.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ARIB_Task_MVC.Interfaces;

namespace ARIB_Task_MVC.Services
{
    public class TaskService:ITaskService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public TaskService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]!); // Ensure this is set in appsettings
            return client;
        }

        public async Task<List<GetTaskDto>> GetTasksAsync()
        {
            var rolesJson = _httpContextAccessor.HttpContext?.Session.GetString("Roles");
            var roles = string.IsNullOrEmpty(rolesJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(rolesJson)!;

            var client = CreateClient();

            var endpoint = roles.Contains("Admin") ? "Task/GetAllTasksForAdmin" : "Task/GetTasksByManager";

            var response = await client.GetAsync($"/api/{endpoint}");

            if (!response.IsSuccessStatusCode)
                return new List<GetTaskDto>();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetTaskResult>(json);

            return result?.Tasks ?? new List<GetTaskDto>();
        }


        public async Task<bool> AddTaskAsync(TaskDto dto)
        {
            var client = CreateClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Task/AddTask", jsonContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<GetEmployeeDto>> GetMyEmployeesAsync()
        {
            var client = CreateClient();
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
                return new List<GetEmployeeDto>();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var employeeIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == "EmployeeId")?.Value;

            if (string.IsNullOrEmpty(employeeIdStr) || !int.TryParse(employeeIdStr, out int employeeId))
                return new List<GetEmployeeDto>();

            var response = await client.GetAsync($"/api/Employee/GetManagersEmployees/{employeeId}");

            if (!response.IsSuccessStatusCode)
                return new List<GetEmployeeDto>();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetEmployeeResult>(json);


            return result?.Employees?.Select(emp => new GetEmployeeDto
            {
                Id = emp.Id,
                FullName = emp.FullName
            }).ToList() ?? new List<GetEmployeeDto>();

        }
        public async Task<List<GetMyTaskDto>> GetMyTasksAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("/api/Task/GetMyTasks");

            if (!response.IsSuccessStatusCode)
                return new List<GetMyTaskDto>();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetMyTaskResult>(json);

            return result?.Tasks ?? new List<GetMyTaskDto>();
        }

        public async Task<bool> ChangeStatusAsync(int id)
        {
            var client = CreateClient();
            var response = await client.PutAsync($"/api/Task/ChangeStatus/{id}", null);
            return response.IsSuccessStatusCode;
        }

    }
}
