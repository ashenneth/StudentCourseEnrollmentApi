using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Dtos.Enrollments;
using StudentCourseEnrollmentApi.Repositories.Interfaces;
using StudentCourseEnrollmentApi.Services.Interfaces;

namespace StudentCourseEnrollmentApi.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollments;
    private readonly IStudentRepository _students;
    private readonly ICourseRepository _courses;

    public EnrollmentService(IEnrollmentRepository enrollments, IStudentRepository students, ICourseRepository courses)
    {
        _enrollments = enrollments;
        _students = students;
        _courses = courses;
    }

    public async Task<ServiceResult<EnrollmentResponseDto>> EnrollAsync(EnrollmentCreateDto dto)
    {
        var student = await _students.GetByIdAsync(dto.StudentId);
        if (student is null) return ServiceResult<EnrollmentResponseDto>.NotFound("Student not found.");

        var course = await _courses.GetByIdAsync(dto.CourseId);
        if (course is null) return ServiceResult<EnrollmentResponseDto>.NotFound("Course not found.");

        var exists = await _enrollments.ExistsAsync(dto.StudentId, dto.CourseId);
        if (exists) return ServiceResult<EnrollmentResponseDto>.Conflict("Student is already enrolled in this course.");

        var enrolledOn = dto.EnrolledOn ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            EnrolledOn = enrolledOn
        };

        await _enrollments.AddAsync(enrollment);
        await _enrollments.SaveChangesAsync();

        // reload data for response
        return ServiceResult<EnrollmentResponseDto>.Created(new EnrollmentResponseDto
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrolledOn = enrollment.EnrolledOn,
            StudentName = student.FullName,
            CourseCode = course.Code
        });
    }

    public async Task<ServiceResult<List<EnrollmentResponseDto>>> GetAllAsync(int? studentId, int? courseId)
    {
        var list = await _enrollments.GetAllAsync(studentId, courseId);
        var data = list.Select(e => new EnrollmentResponseDto
        {
            Id = e.Id,
            StudentId = e.StudentId,
            CourseId = e.CourseId,
            EnrolledOn = e.EnrolledOn,
            StudentName = e.Student?.FullName,
            CourseCode = e.Course?.Code
        }).ToList();

        return ServiceResult<List<EnrollmentResponseDto>>.Ok(data);
    }

    public async Task<ServiceResult<object>> UnenrollAsync(int id)
    {
        var enrollment = await _enrollments.GetByIdAsync(id);
        if (enrollment is null) return ServiceResult<object>.NotFound("Enrollment not found.");

        await _enrollments.DeleteAsync(enrollment);
        await _enrollments.SaveChangesAsync();

        return ServiceResult<object>.NoContent();
    }
}
