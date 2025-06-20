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
    public class GetEmployeeDto:Entity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public double Salary { get; set; }
        public int? ManagerId { get; set; }
        public Employee? Manger { get; set; }
        public ICollection<GetEmployeeDto>? Employees { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? ProfileImageExtension { get; set; } = string.Empty;
        public long? ProfileImageLength { get; set; }
        public GetDepartmentDto? Department { get; set; }
    }
    public class GetEmployeeResult : OperationResult
    {
        public GetEmployeeDto? Employee { get; set; }
        public List<GetEmployeeDto>? Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
