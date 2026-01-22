using Microsoft.EntityFrameworkCore;
using StudentCourseEnrollmentApi.Domain.Entities;

namespace StudentCourseEnrollmentApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(e =>
        {
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(320).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Code).HasMaxLength(10).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Enrollment>(e =>
        {
            e.HasOne(x => x.Student)
             .WithMany(s => s.Enrollments)
             .HasForeignKey(x => x.StudentId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(x => x.CourseId)
             .OnDelete(DeleteBehavior.Cascade);

            // prevents enrolling same student into same course twice
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
        });
    }
}
