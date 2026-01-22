using System.Net;
using StudentCourseEnrollmentApi.Dtos.Courses;
using StudentCourseEnrollmentApi.Repositories;
using StudentCourseEnrollmentApi.Services;

namespace StudentCourseEnrollmentApi.Tests;

public class CourseServiceTests
{
    [Fact]
    public async Task CreateCourse_LowercaseCode_IsUppercasedInResponse()
    {
        using var db = TestDbFactory.CreateContext();

        var repo = new CourseRepository(db);
        var service = new CourseService(repo);

        var dto = new CourseCreateDto
        {
            Title = "Intro to Programming",
            Code = "cs101",
            Credits = 3,
            IsActive = true
        };

        var result = await service.CreateAsync(dto);

        Assert.True(result.Success);
        Assert.Equal("CS101", result.Data!.Code);
    }

    [Fact]
    public async Task CreateCourse_DuplicateCode_ReturnsConflict()
    {
        using var db = TestDbFactory.CreateContext();

        var repo = new CourseRepository(db);
        var service = new CourseService(repo);

        var dto1 = new CourseCreateDto
        {
            Title = "Course 1",
            Code = "CS101",
            Credits = 3,
            IsActive = true
        };

        var dto2 = new CourseCreateDto
        {
            Title = "Course 2",
            Code = "cs101", 
            Credits = 4,
            IsActive = true
        };

        var r1 = await service.CreateAsync(dto1);
        var r2 = await service.CreateAsync(dto2);

        Assert.True(r1.Success);
        Assert.False(r2.Success);
        Assert.Equal(HttpStatusCode.Conflict, r2.StatusCode);
    }
}
