using System.ComponentModel.DataAnnotations;

namespace ARIB_Task_MVC.DTOs
{
    public class DepartmentDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
