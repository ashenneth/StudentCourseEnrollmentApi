using System.ComponentModel.DataAnnotations;

namespace StudentCourseEnrollmentApi.Dtos.Enrollments;

public class EnrollmentCreateDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateOnly? EnrolledOn { get; set; }
}
