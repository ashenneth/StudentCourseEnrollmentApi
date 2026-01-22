using System.ComponentModel.DataAnnotations;

namespace StudentCourseEnrollmentApi.Dtos.Students;

public class StudentCreateDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;
}
