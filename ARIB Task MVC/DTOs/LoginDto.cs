namespace ARIB_Task_MVC.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginResultDto : ApiResponse
    {
        public UserDto? User { get; set; }
    }
}
