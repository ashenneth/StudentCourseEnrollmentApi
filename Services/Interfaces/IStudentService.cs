using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Students;

namespace StudentCourseEnrollmentApi.Services.Interfaces;

public interface IStudentService
{
    Task<ServiceResult<PagedResult<StudentResponseDto>>> GetAllAsync(
        int page = 1,
        int pageSize = 10,
        bool? isActive = null,
        string? search = null);
    Task<ServiceResult<StudentResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<StudentResponseDto>> CreateAsync(StudentCreateDto dto);
    Task<ServiceResult<object>> UpdateAsync(int id, StudentUpdateDto dto);
    Task<ServiceResult<object>> DeleteAsync(int id);
}
