namespace ARIB_Task_MVC.DTOs
{
    public class TaskDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "In Progress";
        public int AssignedToId { get; set; }
    }
}
