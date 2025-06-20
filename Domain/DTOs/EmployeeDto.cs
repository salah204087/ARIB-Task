using DataLayer.Helpers;
using Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EmployeeDto:Entity
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0.")]
        public double Salary { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        [RegularExpression("^(أنثي|ذكر)$", ErrorMessage = "Invalid value for Sex. Allowed values are 'أنثي' or 'ذكر'.")]
        public string Gender { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
    public class EmployeeResult : OperationResult
    {
        public EmployeeDto? Employee { get; set; }
    }
}
