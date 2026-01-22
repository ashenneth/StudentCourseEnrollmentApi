namespace StudentCourseEnrollmentApi.Dtos.Courses;

public class CourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; }
    public bool IsActive { get; set; }
}
