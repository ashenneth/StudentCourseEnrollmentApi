using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Courses;

namespace StudentCourseEnrollmentApi.Services.Interfaces;

public interface ICourseService
{
    Task<ServiceResult<List<CourseResponseDto>>> GetAllAsync();
    Task<ServiceResult<CourseResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<CourseResponseDto>> CreateAsync(CourseCreateDto dto);
    Task<ServiceResult<object>> UpdateAsync(int id, CourseUpdateDto dto);
    Task<ServiceResult<object>> DeleteAsync(int id);
}
