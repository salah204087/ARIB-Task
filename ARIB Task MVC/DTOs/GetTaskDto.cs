namespace ARIB_Task_MVC.DTOs
{
    public class GetTaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
    }
    public class GetTaskResult : ApiResponse
    {
        public GetTaskDto? Task { get; set; }
        public List<GetTaskDto>? Tasks { get; set; }
    }
}
