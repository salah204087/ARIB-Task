using AutoMapper;
using Core.Infrastruture.RepositoryPattern.Repository;
using DataLayer.Models;
using Domain.Common;
using Domain.DTOs;
using Domain.Helper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Domain.Services
{
    public class TaskService(
    IRepository<DataLayer.Models.Task> _taskRepository,
    IRepository<Employee> _employeeRepository,
    IMapper _mapper,
    IHttpContextAccessor _httpContextAccessor
) : ITaskService
    {
        private int? GetEmployeeId()
        {
            var managerIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue("EmployeeId");
            return int.TryParse(managerIdClaim, out int managerId) ? managerId : null;
        }

        public async Task<OperationResult> AddTaskAsync(TaskDto taskDto)
        {
            var result = new OperationResult();
            var managerId = GetEmployeeId();

            if (managerId == null)
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.Unauthorized, "Manager not found in token");

            var employee = await _employeeRepository.FindAsync(e =>
                e.Id == taskDto.AssignedToId && e.ManagerId == managerId.Value);

            if (employee == null)
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.BadRequest, "You can only assign tasks to your own employees.");

            var task = _mapper.Map<DataLayer.Models.Task>(taskDto);
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            task.AssignedById = managerId.Value;
            await _taskRepository.AddAsync(task);

            result.StatusCode = HttpStatusCode.Created;
            result.SuccessMessage = "Task created successfully";
            return result;
        }

        public async Task<GetTaskResult> GetTasksByManagerAsync()
        {
            var result = new GetTaskResult();
            var managerId = GetEmployeeId();

            if (managerId == null)
                return Helper.Helper.CreateErrorResult<GetTaskResult>(
                    HttpStatusCode.Unauthorized, "Manager not found in token");

            var tasks = await _taskRepository.FindAllAsync(
                t => t.AssignedById == managerId.Value, includeProperties: "AssignedTo");

            if (!tasks.Any())
                return Helper.Helper.CreateErrorResult<GetTaskResult>(
                    HttpStatusCode.NotFound, "No tasks found");

            var mapped = tasks.Select(t => new GetTaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                AssignedTo = $"{t.AssignedTo?.FirstName} {t.AssignedTo?.LastName}"
            }).ToList();

            result.Tasks = mapped;
            result.SuccessMessage = "Tasks fetched successfully";
            return result;
        }
        public async Task<GetTaskResult> GetAllTasksForAdminAsync()
        {
            var result = new GetTaskResult();

            var tasks = await _taskRepository.GetAllAsync(includeProperties:"AssignedTo,AssignedBy");

            if (!tasks.Any())
                return Helper.Helper.CreateErrorResult<GetTaskResult>(
                    HttpStatusCode.NotFound, "No tasks found");

            var mapped = tasks.Select(t => new GetTaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                AssignedTo = $"{t.AssignedTo?.FirstName} {t.AssignedTo?.LastName}"
            }).ToList();

            result.Tasks = mapped;
            result.SuccessMessage = "Tasks fetched successfully";
            return result;
        }

        public async Task<GetMyTaskResult> GetMyTasksAsync()
        {
           var result= new GetMyTaskResult();
            var employeeId = GetEmployeeId();

            if (employeeId == null)
                return Helper.Helper.CreateErrorResult<GetMyTaskResult>(
                    HttpStatusCode.Unauthorized, "Manager not found in token");

            var tasks = await _taskRepository.FindAllAsync(
                t => t.AssignedToId == employeeId.Value, includeProperties: "AssignedBy");

            if (!tasks.Any())
                return Helper.Helper.CreateErrorResult<GetMyTaskResult>(
                    HttpStatusCode.NotFound, "No tasks found");

            var mapped = tasks.Select(t => new GetMyTaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                AssignedBy = $"{t.AssignedBy?.FirstName} {t.AssignedBy?.LastName}"
            }).ToList();

            result.Tasks = mapped;
            result.SuccessMessage = "Tasks fetched successfully";
            return result;
        }

        public async Task<OperationResult> ChangeStatus(int id)
        {
            var result = new OperationResult();
            var task =await _taskRepository.GetByIdAsync(id);
            var employeeId = GetEmployeeId();
            if (task == null)
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.NotFound, "Task not found");
            }
            if (task.AssignedToId!=employeeId)
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.Unauthorized, "You can only change the status of your own tasks");
            
            if (task.Status == "Completed")
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.BadRequest, "Task is already completed");
            task.Status = "Completed";
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
            result.StatusCode = HttpStatusCode.OK;
            result.SuccessMessage = "Task status updated successfully";
            return result;
        }

        public async Task<OperationResult> Update(int id, TaskDto taskDto)
        {
            var result = new OperationResult();
            var task =await _taskRepository.FindAsync(n=>n.Id==id,includeProperties: "AssignedTo,AssignedBy");
            if (task == null)
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.NotFound, "Task not found");
            }
            var employeeId = GetEmployeeId();
            if (task.AssignedToId != employeeId)
            {
                return Helper.Helper.CreateErrorResult<OperationResult>(
                    HttpStatusCode.Unauthorized, "You can only update your own tasks");
            }
            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
            task.UpdatedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
            result.StatusCode = HttpStatusCode.OK;
            result.SuccessMessage = MessageEnum.Updated("Task");
            return result;

        }
    }
}
