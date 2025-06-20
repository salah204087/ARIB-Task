using System.ComponentModel.DataAnnotations;

namespace ARIB_Task_MVC.DTOs
{
    public class RegisterationEmployeeDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public int EmployeeId { get; set; }
    }
}
