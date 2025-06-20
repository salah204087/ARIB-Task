using DataLayer.Helpers;
using DataLayer.Models;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetDepartmentDto:Entity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<GetEmployeeDto>? Employees { get; set; }
    }
    public class GetDepartmentResult : OperationResult
    {
        public GetDepartmentDto Department { get; set; }
        public List<GetDepartmentDto> Departments { get; set; } = new();
    }
}
