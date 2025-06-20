using ARIB_Task_MVC.DTOs;
using ARIB_Task_MVC.Interfaces;
using ARIB_Task_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace ARIB_Task_MVC.Controllers
{
    public class AccountController(IAccountService _accountService, IHttpClientFactory _httpClientFactory) : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient("ARIB_API");
            var response = await client.PostAsJsonAsync("Account/Login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ToastMessage"] = "Invalid login.";
                TempData["ToastType"] = "error";
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResultDto>(json);

            if (result != null && result.User != null)
            {
                HttpContext.Session.SetString("Token", result.User.Token);

                HttpContext.Session.SetString("Token", result.User.Token);

                var rolesJson = JsonConvert.SerializeObject(result.User.Role);
                HttpContext.Session.SetString("Roles", rolesJson);

                // خزّن الـ UserId
                HttpContext.Session.SetString("UserId", result.User.Id.ToString());

                HttpContext.Session.SetString("UserId", result.User.Id.ToString());

                return RedirectToAction("MyTasks", "Task");
            }

            TempData["ToastMessage"] = "Something went wrong.";
            TempData["ToastType"] = "error";
            return View();
        }


        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterationEmployeeDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.RegisterEmployeeAsync(model);

            if (!result.Success)
            {
                TempData["ToastMessage"] = result.ErrorMessage ?? "Something went wrong.";
                TempData["ToastType"] = "error";
                return View(model);
            }

            TempData["ToastMessage"] = "Registration successful!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterationAdminDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.RegisterAdminAsync(model);

            if (!result.Success)
            {
                TempData["ToastMessage"] = result.ErrorMessage ?? "Something went wrong.";
                TempData["ToastType"] = "error";
                return View(model);
            }

            TempData["ToastMessage"] = "Admin registered successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                TempData["ToastMessage"] = "Please fill all fields.";
                TempData["ToastType"] = "error";
                return View();
            }

            var result = await _accountService.ChangePasswordAsync(email, currentPassword, newPassword);

            TempData["ToastMessage"] = result.Success ? "Password changed successfully!" : result.ErrorMessage ?? "Failed to change password.";
            TempData["ToastType"] = result.Success ? "success" : "error";

            return View();
        }
        [HttpGet]
        public IActionResult ChangeStatus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string email, bool active)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ToastMessage"] = "Email is required.";
                TempData["ToastType"] = "error";
                return View();
            }

            var result = await _accountService.ChangeStatusOfAccountAsync(email, active);

            TempData["ToastMessage"] = result.Success ? "Account status updated!" : result.ErrorMessage ?? "Failed to update status.";
            TempData["ToastType"] = result.Success ? "success" : "error";

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var result = await _accountService.GetAllUsersAsync();

            if (!result.Success)
            {
                TempData["ToastMessage"] = result.ErrorMessage ?? "Failed to retrieve users.";
                TempData["ToastType"] = "error";
                return View(new List<GetBasicUserDto>());
            }

            return View(result.UsersDto ?? new List<GetBasicUserDto>());
        }



    }
}
