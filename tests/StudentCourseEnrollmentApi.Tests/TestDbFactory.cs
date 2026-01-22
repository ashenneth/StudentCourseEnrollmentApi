using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Data;

namespace StudentCourseEnrollmentApi.Tests;

public static class TestDbFactory
{
    public static AppDbContext CreateContext()
    {
      
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
