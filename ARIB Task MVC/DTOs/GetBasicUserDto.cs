namespace ARIB_Task_MVC.DTOs
{
    public class GetBasicUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
    public class BasicUserResult : ApiResponse
    {
        public List<GetBasicUserDto> UsersDto { get; set; }
    }
}
