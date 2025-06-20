namespace ARIB_Task_MVC.DTOs
{
    public class GetDepartmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<GetEmployeeDto>? Employees { get; set; }
    }
    public class GetDepartmentResult : ApiResponse
    {
        public GetDepartmentDto Department { get; set; } = new();
        public List<GetDepartmentDto> Departments { get; set; } = new();
    }
}
