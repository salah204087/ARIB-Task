using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;
using ARIB_Task_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace ARIB_Task_MVC.Controllers
{
    public class TaskController(ITaskService _taskService, IHttpClientFactory _httpClientFactory) : Controller
    {

        public async Task<IActionResult> Index()
        
        {
            var token = HttpContext.Session.GetString("Token");
            var rolesJson = HttpContext.Session.GetString("Roles");
            var roles = string.IsNullOrEmpty(rolesJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(rolesJson)!;

            if (string.IsNullOrEmpty(token) || !roles.Any())
                return RedirectToAction("Login", "Account");

            var tasks = await _taskService.GetTasksAsync();

            return View(tasks);
        }

        public async Task<IActionResult> Create()
        {
            var employees = await _taskService.GetMyEmployeesAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskDto dto)
        {
            var success = await _taskService.AddTaskAsync(dto);

            if (!success)
            {
                TempData["ToastMessage"] = "Failed to add task.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = "Task created successfully.";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> MyTasks()
        {
            var tasks = await _taskService.GetMyTasksAsync();
            return View(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var client = _httpClientFactory.CreateClient("ARIB_API");

            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ToastMessage"] = "Unauthorized: Please login again.";
                TempData["ToastType"] = "error";
                return RedirectToAction("Login", "Account");
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"Task/ChangeStatus/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "❌ Failed to change status.";
                TempData["ToastType"] = "error";
            }
            else
            {
                TempData["ToastMessage"] = "✅ Status changed successfully.";
                TempData["ToastType"] = "success";
            }

            return RedirectToAction("MyTasks");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var tasks = await _taskService.GetMyTasksAsync();
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
                return NotFound();

            var dto = new TaskDto
            {
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskDto dto)
        {
            var client = _httpClientFactory.CreateClient("ARIB_API");
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ToastMessage"] = "Unauthorized";
                TempData["ToastType"] = "error";
                return RedirectToAction("Login", "Account");
            }

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"Task/Update/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "❌ Failed to update task.";
                TempData["ToastType"] = "error";
                return RedirectToAction("MyTasks");
            }

            TempData["ToastMessage"] = "✅ Task updated successfully.";
            TempData["ToastType"] = "success";
            return RedirectToAction("MyTasks");
        }
    }
}
