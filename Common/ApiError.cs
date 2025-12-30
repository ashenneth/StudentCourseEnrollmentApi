namespace StudentCourseEnrollmentApi.Common;

public sealed class ApiError
{
    public string Code { get; init; } = "error";
    public string Message { get; init; } = "Something went wrong.";
    public object? Details { get; init; }
}

