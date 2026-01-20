using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Domain.Entities;

namespace StudentCourseEnrollmentApi.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<PagedResult<Student>> GetPagedAsync(int page, int pageSize, bool? isActive, string? search);

    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByEmailAsync(string email);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Student student);
    Task SaveChangesAsync();
}
