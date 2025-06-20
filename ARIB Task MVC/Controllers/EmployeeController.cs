using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;
using ARIB_Task_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ARIB_Task_MVC.Controllers
{
    public class EmployeeController(IEmployeeService _service, IDepartmentService _departmentService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllAsync();

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to load employees.";
                TempData["ToastType"] = "error";
                return View(new List<GetEmployeeWithoutImageDto>());
            }

            var result = response as GetEmployeeWithoutImageResult;
            return View(result?.Employees ?? new List<GetEmployeeWithoutImageDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Employee not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            var result = response as GetEmployeeResult;
            return View(result?.Employee);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allEmployeesResponse = await _service.GetListAsync();
            var deptResponse = await _departmentService.GetAllAsync();

            var allEmployees = ((GetListEmployeeResult)allEmployeesResponse)?.Employees?
                .Where(e => e != null && e.Id != 0 && !string.IsNullOrEmpty(e.Name))
                .ToList() ?? new List<GetListEmployeeDto>();

            var departments = ((GetDepartmentResult)deptResponse)?.Departments?
                .Where(d => d != null && d.Id != 0 && !string.IsNullOrEmpty(d.Name))
                .ToList() ?? new List<GetDepartmentDto>();

            ViewBag.Managers = new SelectList(allEmployees, "Id", "Name");
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDto dto)
        {
            var allEmployeesResponse = await _service.GetListAsync();
            var deptResponse = await _departmentService.GetAllAsync();

            var allEmployees = ((GetListEmployeeResult)allEmployeesResponse)?.Employees?
                .Where(e => e != null && e.Id != 0 && !string.IsNullOrEmpty(e.Name))
                .ToList() ?? new List<GetListEmployeeDto>();

            var departments = ((GetDepartmentResult)deptResponse)?.Departments?
                .Where(d => d != null && d.Id != 0 && !string.IsNullOrEmpty(d.Name))
                .ToList() ?? new List<GetDepartmentDto>();

            ViewBag.Managers = new SelectList(allEmployees, "Id", "Name");
            ViewBag.Departments = new SelectList(departments, "Id", "Name");

            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Invalid data submitted.";
                TempData["ToastType"] = "error";
                return View(dto);
            }

            var response = await _service.CreateAsync(dto);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to create employee.";
                TempData["ToastType"] = "error";
                return View(dto);
            }

            TempData["ToastMessage"] = "Employee created successfully.";
            TempData["ToastType"] = "success";

            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _service.GetByIdAsync(id);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Employee not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            var result = response as GetEmployeeResult;

            var dto = new EmployeeDto
            {
                FirstName = result.Employee.FirstName,
                LastName = result.Employee.LastName,
                Gender = result.Employee.Gender,
                Salary = result.Employee.Salary,
                DepartmentId = result.Employee.Department?.Id ?? 0,
                ManagerId = result.Employee.ManagerId
            };

            var allEmployeesResponse = await _service.GetListAsync();
            var deptResponse = await _departmentService.GetAllAsync();

            var allEmployees = ((GetListEmployeeResult)allEmployeesResponse)?.Employees?
                .Where(e => e != null && e.Id != 0 && !string.IsNullOrEmpty(e.Name))
                .ToList() ?? new List<GetListEmployeeDto>();

            var departments = ((GetDepartmentResult)deptResponse)?.Departments?
                .Where(d => d != null && d.Id != 0 && !string.IsNullOrEmpty(d.Name))
                .ToList() ?? new List<GetDepartmentDto>();

            ViewBag.Managers = new SelectList(allEmployees, "Id", "Name");
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.EmployeeId = id;

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] EmployeeDto dto)
        {
            var allEmployeesResponse = await _service.GetListAsync();
            var deptResponse = await _departmentService.GetAllAsync();

            var allEmployees = ((GetListEmployeeResult)allEmployeesResponse)?.Employees?
                .Where(e => e != null && e.Id != 0 && !string.IsNullOrEmpty(e.Name))
                .ToList() ?? new List<GetListEmployeeDto>();

            var departments = ((GetDepartmentResult)deptResponse)?.Departments?
                .Where(d => d != null && d.Id != 0 && !string.IsNullOrEmpty(d.Name))
                .ToList() ?? new List<GetDepartmentDto>();

            ViewBag.Managers = new SelectList(allEmployees, "Id", "Name");
            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.EmployeeId = id;

            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Invalid data submitted.";
                TempData["ToastType"] = "error";
                return View(dto);
            }

            var response = await _service.UpdateAsync(id, dto);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to update employee.";
                TempData["ToastType"] = "error";
                return View(dto);
            }

            TempData["ToastMessage"] = "Employee updated successfully.";
            TempData["ToastType"] = "success";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);

            if (!response.Success)
            {
                TempData["ToastMessage"] = response.ErrorMessage ?? "Failed to delete employee.";
                TempData["ToastType"] = "error";
                return StatusCode((int)response.StatusCode, response.ErrorMessage);
            }

            TempData["ToastMessage"] = "Employee deleted successfully.";
            TempData["ToastType"] = "success";

            return RedirectToAction("Index");
        }
    }
}
