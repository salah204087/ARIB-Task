using Newtonsoft.Json;

namespace ARIB_Task_MVC.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public  string Token { get; set; }
        public List<string> Role { get; set; } = new();
    }
}
