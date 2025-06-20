using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;
using ARIB_Task_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ARIB_Task_MVC.Controllers
{
    public class DepartmentController(IDepartmentService _service) : Controller
    {

       
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to load departments.";
                TempData["ToastType"] = "error";
                return View(new List<GetDepartmentDto>());
            }

            var result = response as GetDepartmentResult;
            return View(result?.Departments ?? new List<GetDepartmentDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Department not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            var result = response as GetDepartmentResult;
            return View(result?.Department);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Invalid data submitted.";
                TempData["ToastType"] = "error";
                return BadRequest("Invalid data");
            }

            var response = await _service.CreateAsync(dto);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to create department.";
                TempData["ToastType"] = "error";
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            TempData["ToastMessage"] = "Department created successfully.";
            TempData["ToastType"] = "success";

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] DepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Invalid data submitted.";
                TempData["ToastType"] = "error";
                return BadRequest("Invalid data");
            }

            var response = await _service.UpdateAsync(id, dto);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to update department.";
                TempData["ToastType"] = "error";
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            TempData["ToastMessage"] = "Department updated successfully.";
            TempData["ToastType"] = "success";

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to delete department.";
                TempData["ToastType"] = "error";
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            TempData["ToastMessage"] = "Department deleted successfully.";
            TempData["ToastType"] = "success";

            return Ok();
        }
    }
}
