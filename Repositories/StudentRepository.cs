using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Data;
using StudentCourseEnrollmentApi.Domain.Entities;
using StudentCourseEnrollmentApi.Repositories.Interfaces;

namespace StudentCourseEnrollmentApi.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _db;
    public StudentRepository(AppDbContext db) => _db = db;

    public Task<List<Student>> GetAllAsync() =>
        _db.Students.AsNoTracking().OrderBy(s => s.Id).ToListAsync();

    public Task<Student?> GetByIdAsync(int id) =>
        _db.Students.FirstOrDefaultAsync(s => s.Id == id);

    public Task<Student?> GetByEmailAsync(string email) =>
        _db.Students.FirstOrDefaultAsync(s => s.Email == email);

    public Task AddAsync(Student student) => _db.Students.AddAsync(student).AsTask();

    public Task UpdateAsync(Student student)
    {
        _db.Students.Update(student);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Student student)
    {
        _db.Students.Remove(student);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
