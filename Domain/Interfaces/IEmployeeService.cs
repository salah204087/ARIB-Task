using Domain.Common;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmployeeService
    {
        Task<GetEmployeeWithoutImageResult> GetAllAsync();
        Task<GetListEmployeeResult> GetListEmployeesAsync();
        Task<GetEmployeeResult> GetManagersEmployeesAsync(int id);
        Task<GetEmployeeResult> GetAsync(int id);
        Task<EmployeeResult> CreateAsync(EmployeeDto employeeDto);
        Task<EmployeeResult> UpdateAsync(int id, EmployeeDto employeeDto);
        Task<OperationResult> DeleteAsync(int id);
    }
}
