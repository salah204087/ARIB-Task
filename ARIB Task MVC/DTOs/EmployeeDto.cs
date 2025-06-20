using System.ComponentModel.DataAnnotations;

namespace ARIB_Task_MVC.DTOs
{
    public class EmployeeDto
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
}
