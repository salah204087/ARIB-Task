using Domain.DTOs;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARIB_Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController(ITaskService _taskService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetTasksByManager()
        {
            var result = await _taskService.GetTasksByManagerAsync();

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllTasksForAdmin()
        {
            var result = await _taskService.GetAllTasksForAdminAsync();

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> AddTask(TaskDto taskDto)
        {
            var result = await _taskService.AddTaskAsync(taskDto);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetMyTasks()
        {
            var result = await _taskService.GetMyTasksAsync();

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> ChangeStatus(int id)
        {
            var result = await _taskService.ChangeStatus(id);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Update(int id, TaskDto taskDto)
        {
            var result = await _taskService.Update(id,taskDto);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
    }
}
