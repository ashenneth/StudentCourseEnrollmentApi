using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Dtos.Students;
using StudentCourseEnrollmentApi.Repositories.Interfaces;
using StudentCourseEnrollmentApi.Services.Interfaces;

namespace StudentCourseEnrollmentApi.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;
    public StudentService(IStudentRepository repo) => _repo = repo;

    public async Task<ServiceResult<PagedResult<StudentResponseDto>>> GetAllAsync(
        int page,
        int pageSize,
        bool? isActive,
        string? search)
    {
        if (page < 1) return ServiceResult<PagedResult<StudentResponseDto>>.BadRequest("page must be >= 1");
        if (pageSize < 1 || pageSize > 100) return ServiceResult<PagedResult<StudentResponseDto>>.BadRequest("pageSize must be between 1 and 100");

        var paged = await _repo.GetPagedAsync(page, pageSize, isActive, search);

        var dtoPaged = new PagedResult<StudentResponseDto>
        {
            Items = paged.Items.Select(MapToResponse).ToList(),
            Page = paged.Page,
            PageSize = paged.PageSize,
            TotalCount = paged.TotalCount
        };

        return ServiceResult<PagedResult<StudentResponseDto>>.Ok(dtoPaged);
    }


    public async Task<ServiceResult<StudentResponseDto>> GetByIdAsync(int id)
    {
        var s = await _repo.GetByIdAsync(id);
        if (s is null) return ServiceResult<StudentResponseDto>.NotFound("Student not found.");
        return ServiceResult<StudentResponseDto>.Ok(MapToResponse(s));
    }

    public async Task<ServiceResult<StudentResponseDto>> CreateAsync(StudentCreateDto dto)
    {
        var existing = await _repo.GetByEmailAsync(dto.Email.Trim());
        if (existing is not null) return ServiceResult<StudentResponseDto>.Conflict("Email already exists.");

        var student = new Student
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            DateOfBirth = dto.DateOfBirth,
            IsActive = dto.IsActive
        };

        await _repo.AddAsync(student);
        await _repo.SaveChangesAsync();

        return ServiceResult<StudentResponseDto>.Created(MapToResponse(student));
    }

    public async Task<ServiceResult<object>> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student is null) return ServiceResult<object>.NotFound("Student not found.");

        var normalizedEmail = dto.Email.Trim();
        var emailOwner = await _repo.GetByEmailAsync(normalizedEmail);
        if (emailOwner is not null && emailOwner.Id != id)
            return ServiceResult<object>.Conflict("Email already exists.");

        student.FullName = dto.FullName.Trim();
        student.Email = normalizedEmail;
        student.DateOfBirth = dto.DateOfBirth;
        student.IsActive = dto.IsActive;

        await _repo.UpdateAsync(student);
        await _repo.SaveChangesAsync();

        return ServiceResult<object>.NoContent();
    }

    public async Task<ServiceResult<object>> DeleteAsync(int id)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student is null) return ServiceResult<object>.NotFound("Student not found.");

        await _repo.DeleteAsync(student);
        await _repo.SaveChangesAsync();

        return ServiceResult<object>.NoContent();
    }

    private static StudentResponseDto MapToResponse(Student s) => new()
    {
        Id = s.Id,
        FullName = s.FullName,
        Email = s.Email,
        DateOfBirth = s.DateOfBirth,
        IsActive = s.IsActive
    };
}
