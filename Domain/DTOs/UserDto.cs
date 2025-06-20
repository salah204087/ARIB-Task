using DataLayer.Helpers;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserDto : Entity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public List<string> Role { get; set; } = new();
    }
    public class UserResult : OperationResult
    {
        public UserDto User { get; set; }
    }
}
