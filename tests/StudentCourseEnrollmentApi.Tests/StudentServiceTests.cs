using System.Net;
using StudentCourseEnrollmentApi.Dtos.Students;
using StudentCourseEnrollmentApi.Repositories;
using StudentCourseEnrollmentApi.Services;

namespace StudentCourseEnrollmentApi.Tests;

public class StudentServiceTests
{
    [Fact]
    public async Task CreateStudent_DuplicateEmail_ReturnsConflict()
    {
        using var db = TestDbFactory.CreateContext();

        var repo = new StudentRepository(db);
        var service = new StudentService(repo);

        var dto1 = new StudentCreateDto
        {
            FullName = "Nimal Perera",
            Email = "nimal@example.com",
            DateOfBirth = new DateOnly(2001, 4, 15),
            IsActive = true
        };

        var dto2 = new StudentCreateDto
        {
            FullName = "Kasun Silva",
            Email = "nimal@example.com", // duplicate
            DateOfBirth = new DateOnly(2000, 1, 1),
            IsActive = true
        };

        var r1 = await service.CreateAsync(dto1);
        var r2 = await service.CreateAsync(dto2);

        Assert.True(r1.Success);
        Assert.False(r2.Success);
        Assert.Equal(HttpStatusCode.Conflict, r2.StatusCode);
    }
}
