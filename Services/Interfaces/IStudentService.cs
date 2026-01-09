using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Dtos.Students;

namespace StudentCourseEnrollmentApi.Services.Interfaces;

public interface IStudentService
{
    Task<ServiceResult<List<StudentResponseDto>>> GetAllAsync();
    Task<ServiceResult<StudentResponseDto>> GetByIdAsync(int id);
    Task<ServiceResult<StudentResponseDto>> CreateAsync(StudentCreateDto dto);
    Task<ServiceResult<object>> UpdateAsync(int id, StudentUpdateDto dto);
    Task<ServiceResult<object>> DeleteAsync(int id);
}
