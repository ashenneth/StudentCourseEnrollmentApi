using System.Net;

namespace StudentCourseEnrollmentApi.Common;

public sealed class ServiceResult<T>
{
    public bool Success { get; private init; }
    public HttpStatusCode StatusCode { get; private init; }
    public T? Data { get; private init; }
    public ApiError? Error { get; private init; }

    public static ServiceResult<T> Ok(T data) => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.OK,
        Data = data
    };

    public static ServiceResult<T> Created(T data) => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.Created,
        Data = data
    };

    public static ServiceResult<T> NoContent() => new()
    {
        Success = true,
        StatusCode = HttpStatusCode.NoContent
    };

    public static ServiceResult<T> NotFound(string message) => new()
    {
        Success = false,
        StatusCode = HttpStatusCode.NotFound,
        Error = new ApiError { Code = "not_found", Message = message }
    };

    public static ServiceResult<T> Conflict(string message) => new()
    {
        Success = false,
        StatusCode = HttpStatusCode.Conflict,
        Error = new ApiError { Code = "conflict", Message = message }
    };

    public static ServiceResult<T> BadRequest(string message, object? details = null) => new()
    {
        Success = false,
        StatusCode = HttpStatusCode.BadRequest,
        Error = new ApiError { Code = "bad_request", Message = message, Details = details }
    };
}
