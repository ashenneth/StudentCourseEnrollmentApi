namespace StudentCourseEnrollmentApi.Domain.Entities;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public bool IsActive { get; set; } = true;

    public List<Enrollment> Enrollments { get; set; } = new();
}
