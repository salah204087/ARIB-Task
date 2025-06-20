using Domain.Common;
using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITaskService
    {
        Task<OperationResult> AddTaskAsync(TaskDto taskDto);
        Task<GetTaskResult> GetTasksByManagerAsync();
        Task<GetTaskResult> GetAllTasksForAdminAsync();
        Task<GetMyTaskResult> GetMyTasksAsync();
        Task<OperationResult> ChangeStatus(int id);
        Task<OperationResult> Update(int id,TaskDto taskDto);
    }
}
