using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Common;
using StudentCourseEnrollmentApi.Data;
using StudentCourseEnrollmentApi.Repositories.Interfaces;
using StudentCourseEnrollmentApi.Repositories;
using StudentCourseEnrollmentApi.Services.Interfaces;
using StudentCourseEnrollmentApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON DateOnly support
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(cs);
});

// DI - Repositories
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

// DI - Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
