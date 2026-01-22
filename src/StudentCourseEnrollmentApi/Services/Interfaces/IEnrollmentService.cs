using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Enrollments;

namespace StudentCourseEnrollmentApi.Services.Interfaces;

public interface IEnrollmentService
{
    Task<ServiceResult<EnrollmentResponseDto>> EnrollAsync(EnrollmentCreateDto dto);
    Task<ServiceResult<List<EnrollmentResponseDto>>> GetAllAsync(int? studentId, int? courseId);
    Task<ServiceResult<object>> UnenrollAsync(int id);
}
