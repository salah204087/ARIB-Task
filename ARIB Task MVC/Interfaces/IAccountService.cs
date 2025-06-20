using ARIB_Task_MVC.DTOs;

namespace ARIB_Task_MVC.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
        Task<ApiResponse> RegisterEmployeeAsync(RegisterationEmployeeDto model);
        Task<ApiResponse> RegisterAdminAsync(RegisterationAdminDto model);
        Task<ApiResponse> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        Task<ApiResponse> ChangeStatusOfAccountAsync(string email, bool active);
        Task<BasicUserResult> GetAllUsersAsync();
    }
}
