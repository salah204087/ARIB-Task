using DataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Department:Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Employee>? Employees { get; set; }
    }
}
