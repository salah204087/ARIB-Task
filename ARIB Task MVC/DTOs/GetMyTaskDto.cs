namespace ARIB_Task_MVC.DTOs
{
    public class GetMyTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AssignedBy { get; set; } = string.Empty;
    }
    public class GetMyTaskResult : ApiResponse
    {
        public GetMyTaskDto? Task { get; set; }
        public List<GetMyTaskDto>? Tasks { get; set; }
    }
}

