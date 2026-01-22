using System.Net;
using StudentCourseEnrollmentApi.Dtos.Courses;
using StudentCourseEnrollmentApi.Dtos.Enrollments;
using StudentCourseEnrollmentApi.Dtos.Students;
using StudentCourseEnrollmentApi.Repositories;
using StudentCourseEnrollmentApi.Services;

namespace StudentCourseEnrollmentApi.Tests;

public class EnrollmentServiceTests
{
    [Fact]
    public async Task Enroll_SameStudentSameCourseTwice_ReturnsConflict()
    {
        using var db = TestDbFactory.CreateContext();

        var studentRepo = new StudentRepository(db);
        var courseRepo = new CourseRepository(db);
        var enrollmentRepo = new EnrollmentRepository(db);

        var studentService = new StudentService(studentRepo);
        var courseService = new CourseService(courseRepo);
        var enrollmentService = new EnrollmentService(enrollmentRepo, studentRepo, courseRepo);

        // create student
        var s = await studentService.CreateAsync(new StudentCreateDto
        {
            FullName = "Nimal Perera",
            Email = "nimal@example.com",
            DateOfBirth = new DateOnly(2001, 4, 15),
            IsActive = true
        });

        // create course
        var c = await courseService.CreateAsync(new CourseCreateDto
        {
            Title = "Intro to Programming",
            Code = "CS101",
            Credits = 3,
            IsActive = true
        });

        // enroll once
        var e1 = await enrollmentService.EnrollAsync(new EnrollmentCreateDto
        {
            StudentId = s.Data!.Id,
            CourseId = c.Data!.Id,
            EnrolledOn = new DateOnly(2025, 1, 10)
        });

        // enroll again (same student + course)
        var e2 = await enrollmentService.EnrollAsync(new EnrollmentCreateDto
        {
            StudentId = s.Data!.Id,
            CourseId = c.Data!.Id,
            EnrolledOn = new DateOnly(2025, 1, 10)
        });

        Assert.True(e1.Success);
        Assert.False(e2.Success);
        Assert.Equal(HttpStatusCode.Conflict, e2.StatusCode);
    }
}
