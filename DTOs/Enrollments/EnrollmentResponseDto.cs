namespace StudentCourseEnrollmentApi.Dtos.Enrollments;

public class EnrollmentResponseDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateOnly EnrolledOn { get; set; }

    public string? StudentName { get; set; }
    public string? CourseCode { get; set; }
}
