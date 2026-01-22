using Microsoft.AspNetCore.Mvc;
using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Enrollments;
using StudentCourseEnrollmentApi.Services.Interfaces;

namespace StudentCourseEnrollmentApi.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;
    public EnrollmentsController(IEnrollmentService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<EnrollmentResponseDto>> Enroll([FromBody] EnrollmentCreateDto dto)
    {
        var result = await _service.EnrollAsync(dto);
        if (result.Success)
            return StatusCode(201, result.Data);

        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<EnrollmentResponseDto>>> GetAll([FromQuery] int? studentId, [FromQuery] int? courseId)
    {
        var result = await _service.GetAllAsync(studentId, courseId);
        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Unenroll(int id)
    {
        var result = await _service.UnenrollAsync(id);
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
