using Microsoft.AspNetCore.Mvc;
using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Students;
using StudentCourseEnrollmentApi.Services.Interfaces;

namespace StudentCourseEnrollmentApi.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;
    public StudentsController(IStudentService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<StudentResponseDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentResponseDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<StudentResponseDto>> Create([FromBody] StudentCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);

        return ToActionResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return ToActionResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return ToActionResult(result);
    }

    private ActionResult ToActionResult<T>(ServiceResult<T> result)
    {
        if (result.Success)
        {
            if (result.StatusCode == System.Net.HttpStatusCode.NoContent) return NoContent();
            if (result.StatusCode == System.Net.HttpStatusCode.Created) return StatusCode(201, result.Data);
            return Ok(result.Data);
        }

        return result.StatusCode switch
        {
            System.Net.HttpStatusCode.NotFound => NotFound(result.Error),
            System.Net.HttpStatusCode.Conflict => Conflict(result.Error),
            System.Net.HttpStatusCode.BadRequest => BadRequest(result.Error),
            _ => StatusCode(500, new ApiError { Code = "server_error", Message = "Unexpected error." })
        };
    }
}
