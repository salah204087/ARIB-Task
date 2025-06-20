using DataLayer.Helpers;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DepartmentDto:Entity
    {
        [Required]
        public string Name { get; set; } = null!;
    }
    public class DepartmentResult : OperationResult
    {
        public DepartmentDto Department { get; set; }
    }

}
