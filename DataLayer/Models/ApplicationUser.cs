using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public bool IsActive { get; set; }
    }
}
