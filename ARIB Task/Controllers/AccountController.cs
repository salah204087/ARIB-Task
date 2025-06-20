using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ARIB_Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController(IAccountService _accountService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterEmployee(RegisterationEmployeeDto registerationDto)
        {
            var result = await _accountService.RegisterEmployeeAsync(registerationDto);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RegisterAdmin(RegisterationAdminDto registerationDto)
        {
            var result = await _accountService.RegisterAdminAsync(registerationDto);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var result = await _accountService.ChangePasswordAsync(email, currentPassword, newPassword);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangeStatusOfAccount([Required] string email, [Required] bool active)
        {
            var result = await _accountService.ChangeStatusOfAccount(email, active);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetRoles()
        {
            var result = _accountService.GetRoles();
            return Ok(result);
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllUser()
        {
            var result = await _accountService.GetAllUsersAsync();
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _accountService.GetUserByIdAsync(id);

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
    }
}
