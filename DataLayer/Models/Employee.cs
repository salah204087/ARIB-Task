using DataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Employee:Entity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string Gender { get; set; } = string.Empty;
        public double Salary { get; set; }
        public int? ManagerId { get; set; }
        public Employee? Manger { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? ProfileImageExtension { get; set; } = string.Empty;
        public long? ProfileImageLength { get; set; }
    }
}
