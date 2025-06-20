using ARIB_Task_MVC.DTOs;

namespace ARIB_Task_MVC.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetByIdAsync(int id);
        Task<ApiResponse> CreateAsync(DepartmentDto dto);
        Task<ApiResponse> UpdateAsync(int id, DepartmentDto dto);
        Task<ApiResponse> DeleteAsync(int id);
    }
}
