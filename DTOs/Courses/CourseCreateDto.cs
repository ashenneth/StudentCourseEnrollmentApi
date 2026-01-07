using System.ComponentModel.DataAnnotations;

namespace StudentCourseEnrollmentApi.Dtos.Courses;

public class CourseCreateDto
{
    [Required, StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(10, MinimumLength = 4)]
    public string Code { get; set; } = string.Empty;

    [Range(1, 6)]
    public int Credits { get; set; }

    public bool IsActive { get; set; } = true;
}
