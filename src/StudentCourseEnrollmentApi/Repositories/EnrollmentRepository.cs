using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Data;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Repositories.Interfaces;

namespace StudentCourseEnrollmentApi.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _db;
    public EnrollmentRepository(AppDbContext db) => _db = db;

    public async Task<List<Enrollment>> GetAllAsync(int? studentId = null, int? courseId = null)
    {
        var q = _db.Enrollments
            .AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .AsQueryable();

        if (studentId.HasValue) q = q.Where(e => e.StudentId == studentId.Value);
        if (courseId.HasValue) q = q.Where(e => e.CourseId == courseId.Value);

        return await q.OrderBy(e => e.Id).ToListAsync();
    }

    public Task<Enrollment?> GetByIdAsync(int id) =>
        _db.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);

    public Task<bool> ExistsAsync(int studentId, int courseId) =>
        _db.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

    public Task AddAsync(Enrollment enrollment) => _db.Enrollments.AddAsync(enrollment).AsTask();

    public Task DeleteAsync(Enrollment enrollment)
    {
        _db.Enrollments.Remove(enrollment);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
