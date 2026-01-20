using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Common;
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
    
    public async Task<PagedResult<Student>> GetPagedAsync(int page, int pageSize, bool? isActive, string? search)
    {
        var q = _db.Students.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
            q = q.Where(s => s.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            q = q.Where(s =>
                s.FullName.ToLower().Contains(term) ||
                s.Email.ToLower().Contains(term));
        }

        var totalCount = await q.CountAsync();

        var items = await q
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Student>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

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
