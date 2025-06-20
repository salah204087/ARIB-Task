using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetMyTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AssignedBy { get; set; } = string.Empty;
    }
    public class GetMyTaskResult : OperationResult
    {
        public GetMyTaskDto? Task { get; set; }
        public List<GetMyTaskDto>? Tasks { get; set; }
    }
}
