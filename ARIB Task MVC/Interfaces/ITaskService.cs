using ARIB_Task_MVC.DTOs;

namespace ARIB_Task_MVC.Interfaces
{
    public interface ITaskService
    {
        Task<List<GetTaskDto>> GetTasksAsync();
        Task<bool> AddTaskAsync(TaskDto dto);
        Task<List<GetEmployeeDto>> GetMyEmployeesAsync();
        Task<List<GetMyTaskDto>> GetMyTasksAsync();
        Task<bool> ChangeStatusAsync(int id);
    }
}
