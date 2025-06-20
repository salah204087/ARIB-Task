namespace ARIB_Task_MVC.DTOs
{
    public class GetListEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetListEmployeeResult : ApiResponse
    {
        public List<GetListEmployeeDto>? Employees { get; set; }
    }
}
