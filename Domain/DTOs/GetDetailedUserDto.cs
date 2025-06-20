using DataLayer.Helpers;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetDetailedUserDto : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Roles { get; set; }
    }
    public class DetailUserResult : OperationResult
    {
        public GetDetailedUserDto GetDetailedUser { get; set; }
    }
}
