using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARIB_Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAll()
        {
            var result = await _employeeService.GetAllAsync();
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetListEmployees()
        {
            var result = await _employeeService.GetListEmployeesAsync();
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _employeeService.GetAsync(id);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromForm] EmployeeDto employeeDto)
        {
            var result = await _employeeService.CreateAsync(employeeDto);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, [FromForm] EmployeeDto employeeDto)
        {
            var result = await _employeeService.UpdateAsync(id, employeeDto);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteAsync(id);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetManagersEmployees(int id)
        {
            var result = await _employeeService.GetManagersEmployeesAsync(id);
            if (!result.Success)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
