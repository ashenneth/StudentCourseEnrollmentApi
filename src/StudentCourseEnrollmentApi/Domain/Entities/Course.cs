namespace StudentCourseEnrollmentApi.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // uppercase enforced in service
    public int Credits { get; set; }
    public bool IsActive { get; set; } = true;

    public List<Enrollment> Enrollments { get; set; } = new();
}
