using Domain.Common;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDepartmentService
    {
        Task<GetDepartmentResult> GetAllAsync();
        Task<GetDepartmentResult> GetByIdAsync(int id);
        Task<DepartmentResult> CreateAsync(DepartmentDto departmentDto);
        Task<DepartmentResult> UpdateAsync(int id, DepartmentDto departmentDto);
        Task<OperationResult> DeleteAsync(int id);
    }
}
