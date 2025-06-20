namespace ARIB_Task_MVC.DTOs
{
    public class GetEmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public double Salary { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public List<GetEmployeeDto>? Employees { get; set; }
        public byte[]? ProfileImage { get; set; }
        public string? ProfileImageExtension { get; set; }
        public long? ProfileImageLength { get; set; }
        public GetDepartmentDto? Department { get; set; }
    }

    public class GetEmployeeResult : ApiResponse
    {
        public GetEmployeeDto? Employee { get; set; }
        public List<GetEmployeeDto>? Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
