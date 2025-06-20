using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetListEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class GetListEmployeeResult : OperationResult
    {
        public List<GetListEmployeeDto> Employees { get; set; }
    }
}
