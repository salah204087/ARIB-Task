using DataLayer.Helpers;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetEmployeeWithoutImageDto : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GetDepartmentDto? Department { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public ICollection<GetEmployeeDto>? Employees { get; set; }
    }
    public class GetEmployeeWithoutImageResult : OperationResult
    {
        public GetEmployeeWithoutImageDto? Employee { get; set; }
        public List<GetEmployeeWithoutImageDto>? Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
