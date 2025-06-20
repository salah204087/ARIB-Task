using DataLayer.Helpers;
using DataLayer.Models;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TaskDto : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public int AssignedToId { get; set; }
    }
    public class TaskResult : OperationResult
    {
        public TaskDto? Task { get; set; }
    }
}
