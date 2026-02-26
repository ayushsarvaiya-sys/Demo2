using Backend.DTOs;
using Backend.Services.Interfaces;
using Backend.Utiles;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            try
            {
                var data = await _service.GetAllAsync(skip, take);
                return Ok(new ApiResponse(200, "Employees retrieved successfully", data));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return Ok(new ApiResponse(200, "Employee retrieved successfully", data));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDto dto)
        {
            try
            {
                var data = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = data.Id }, 
                    new ApiResponse(201, "Employee created successfully", data));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto dto)
        {
            try
            {
                var data = await _service.UpdateAsync(id, dto);
                return Ok(new ApiResponse(200, "Employee updated successfully", data));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                var count = await _service.GetCountAsync();
                return Ok(new ApiResponse(200, "Count retrieved successfully", new { count }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? departmentId,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 10)
        {
            try
            {
                var data = await _service.SearchAsync(searchTerm, startDate, endDate, departmentId, skip, take);
                return Ok(new ApiResponse(200, "Search completed successfully", data));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiError(400, ex.Message));
            }
        }
    }
}
