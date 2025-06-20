using DataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegisterationEmployeeDto : Entity
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
