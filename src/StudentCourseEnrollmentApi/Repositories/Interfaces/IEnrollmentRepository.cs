using StudentCourseEnrollmentApi.Domain.Entities;

namespace StudentCourseEnrollmentApi.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetAllAsync(int? studentId = null, int? courseId = null);
    Task<Enrollment?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int studentId, int courseId);
    Task AddAsync(Enrollment enrollment);
    Task DeleteAsync(Enrollment enrollment);
    Task SaveChangesAsync();
}
