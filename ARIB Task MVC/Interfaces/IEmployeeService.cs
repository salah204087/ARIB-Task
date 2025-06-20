using ARIB_Task_MVC.DTOs;

namespace ARIB_Task_MVC.Interfaces
{
    public interface IEmployeeService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetListAsync();
        Task<GetEmployeeResult> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(EmployeeDto dto);
        Task<ApiResponse> UpdateAsync(int id, EmployeeDto dto);
        Task<ApiResponse> DeleteAsync(int id);
    }
}
