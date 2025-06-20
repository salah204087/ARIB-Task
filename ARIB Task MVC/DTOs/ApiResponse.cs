namespace ARIB_Task_MVC.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public object? Data { get; set; }
        public int StatusCode { get; set; }
    }
}
