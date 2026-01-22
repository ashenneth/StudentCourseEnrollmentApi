using System.Net;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Common;

namespace StudentCourseEnrollmentApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbUpdateException ex) when (IsSqliteUniqueConstraint(ex))
        {
            _logger.LogWarning(ex, "Database constraint violation.");
            await WriteError(context, HttpStatusCode.Conflict,
                new ApiError { Code = "conflict", Message = "A unique constraint was violated." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");
            await WriteError(context, HttpStatusCode.InternalServerError,
                new ApiError { Code = "server_error", Message = "Unexpected error occurred." });
        }
    }

    private static bool IsSqliteUniqueConstraint(DbUpdateException ex)
    {
        var sqliteEx = ex.InnerException as SqliteException;
        return sqliteEx?.SqliteErrorCode == 19;
    }

    private static async Task WriteError(HttpContext context, HttpStatusCode status, ApiError error)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(error, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
