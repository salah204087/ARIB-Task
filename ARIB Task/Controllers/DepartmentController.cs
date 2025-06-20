using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARIB_Task.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController(IDepartmentService _departmentService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAll()
        {
            var result = await _departmentService.GetAllAsync();

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);

            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create(DepartmentDto departmentDto)
        {
            var result = await _departmentService.CreateAsync(departmentDto);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Update(int id, DepartmentDto departmentDto)
        {
            var result = await _departmentService.UpdateAsync(id, departmentDto);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _departmentService.DeleteAsync(id);
            return result.Success
                ? StatusCode((int)result.StatusCode, result)
                : StatusCode((int)result.StatusCode, result.ErrorMessage);
        }
    }
}
