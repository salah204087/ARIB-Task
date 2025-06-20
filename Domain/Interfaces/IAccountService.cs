using Domain.Common;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAccountService
    {
        Task<UserResult> LoginAsync(LoginDto loginRequestDTO);
        Task<UserResult> RegisterEmployeeAsync(RegisterationEmployeeDto registerationDto);
        Task<UserResult> RegisterAdminAsync(RegisterationAdminDto registerationDto);
        Task<OperationResult> ChangePasswordAsync(string email, string currentPassword, string newPassword);
        List<string> GetRoles();
        Task<OperationResult> ChangeStatusOfAccount(string email, bool active);
        Task<BasicUserResult> GetAllUsersAsync();
        Task<DetailUserResult> GetUserByIdAsync(int userId);
    }
}
