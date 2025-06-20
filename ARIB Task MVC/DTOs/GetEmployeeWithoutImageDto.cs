namespace ARIB_Task_MVC.DTOs
{
    public class GetEmployeeWithoutImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public GetDepartmentDto? Department { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; } = string.Empty;
        public List<GetEmployeeDto>? Employees { get; set; }
    }

    public class GetEmployeeWithoutImageResult : ApiResponse
    {
        public GetEmployeeWithoutImageDto? Employee { get; set; }
        public List<GetEmployeeWithoutImageDto>? Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
