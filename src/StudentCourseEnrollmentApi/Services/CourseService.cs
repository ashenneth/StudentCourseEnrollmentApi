using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Dtos.Courses;
using StudentCourseEnrollmentApi.Repositories.Interfaces;
using StudentCourseEnrollmentApi.Services.Interfaces;

namespace StudentCourseEnrollmentApi.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repo;
    public CourseService(ICourseRepository repo) => _repo = repo;

    public async Task<ServiceResult<List<CourseResponseDto>>> GetAllAsync()
    {
        var courses = await _repo.GetAllAsync();
        return ServiceResult<List<CourseResponseDto>>.Ok(courses.Select(MapToResponse).ToList());
    }

    public async Task<ServiceResult<CourseResponseDto>> GetByIdAsync(int id)
    {
        var c = await _repo.GetByIdAsync(id);
        if (c is null) return ServiceResult<CourseResponseDto>.NotFound("Course not found.");
        return ServiceResult<CourseResponseDto>.Ok(MapToResponse(c));
    }

    public async Task<ServiceResult<CourseResponseDto>> CreateAsync(CourseCreateDto dto)
    {
        var code = NormalizeCourseCode(dto.Code);
        if (code is null)
            return ServiceResult<CourseResponseDto>.BadRequest("Course code must be 4–10 characters and contain only letters/numbers.");

        var existing = await _repo.GetByCodeAsync(code);
        if (existing is not null) return ServiceResult<CourseResponseDto>.Conflict("Course code already exists.");

        var course = new Course
        {
            Title = dto.Title.Trim(),
            Code = code,
            Credits = dto.Credits,
            IsActive = dto.IsActive
        };

        await _repo.AddAsync(course);
        await _repo.SaveChangesAsync();

        return ServiceResult<CourseResponseDto>.Created(MapToResponse(course));
    }

    public async Task<ServiceResult<object>> UpdateAsync(int id, CourseUpdateDto dto)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return ServiceResult<object>.NotFound("Course not found.");

        var code = NormalizeCourseCode(dto.Code);
        if (code is null)
            return ServiceResult<object>.BadRequest("Course code must be 4–10 characters and contain only letters/numbers.");

        var codeOwner = await _repo.GetByCodeAsync(code);
        if (codeOwner is not null && codeOwner.Id != id)
            return ServiceResult<object>.Conflict("Course code already exists.");

        course.Title = dto.Title.Trim();
        course.Code = code;
        course.Credits = dto.Credits;
        course.IsActive = dto.IsActive;

        await _repo.UpdateAsync(course);
        await _repo.SaveChangesAsync();

        return ServiceResult<object>.NoContent();
    }

    public async Task<ServiceResult<object>> DeleteAsync(int id)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course is null) return ServiceResult<object>.NotFound("Course not found.");

        await _repo.DeleteAsync(course);
        await _repo.SaveChangesAsync();

        return ServiceResult<object>.NoContent();
    }

    private static CourseResponseDto MapToResponse(Course c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Code = c.Code,
        Credits = c.Credits,
        IsActive = c.IsActive
    };

    private static string? NormalizeCourseCode(string input)
    {
        var code = (input ?? string.Empty).Trim().ToUpperInvariant();
        if (code.Length < 4 || code.Length > 10) return null;
        if (!code.All(ch => char.IsLetterOrDigit(ch))) return null;
        return code;
    }
}
