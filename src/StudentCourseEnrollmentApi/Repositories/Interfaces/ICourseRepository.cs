using StudentCourseEnrollmentApi.Domain.Entities;

namespace StudentCourseEnrollmentApi.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetByCodeAsync(string code);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
    Task SaveChangesAsync();
}
