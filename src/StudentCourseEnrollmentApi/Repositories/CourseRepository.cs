using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Data;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Repositories.Interfaces;

namespace StudentCourseEnrollmentApi.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _db;
    public CourseRepository(AppDbContext db) => _db = db;

    public Task<List<Course>> GetAllAsync() =>
        _db.Courses.AsNoTracking().OrderBy(c => c.Id).ToListAsync();

    public Task<Course?> GetByIdAsync(int id) =>
        _db.Courses.FirstOrDefaultAsync(c => c.Id == id);

    public Task<Course?> GetByCodeAsync(string code) =>
        _db.Courses.FirstOrDefaultAsync(c => c.Code == code);

    public Task AddAsync(Course course) => _db.Courses.AddAsync(course).AsTask();

    public Task UpdateAsync(Course course)
    {
        _db.Courses.Update(course);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Course course)
    {
        _db.Courses.Remove(course);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
