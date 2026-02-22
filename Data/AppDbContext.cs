using Microsoft.EntityFrameworkCore;
using OnlineCourseCatalog.Models;

namespace OnlineCourseCatalog.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 🔁 Self reference Topic
        modelBuilder.Entity<Topic>()
            .HasOne(t => t.Parent)
            .WithMany(t => t.Children)
            .HasForeignKey(t => t.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🔗 Course → User
        modelBuilder.Entity<Course>()
            .HasOne(c => c.CreatedBy)
            .WithMany(u => u.Courses)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        // 💰 Decimal precision
        modelBuilder.Entity<Course>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Course>()
            .Property(c => c.DiscountRate)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Course>()
            .Property(c => c.UpdatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Course>()
            .Property(c => c.CreatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Course>()
            .Property(c => c.DeletedAt)
            .HasColumnType("timestamp without time zone");

         modelBuilder.Entity<Language>()
            .Property(c => c.UpdatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Language>()
            .Property(c => c.CreatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Language>()
            .Property(c => c.DeletedAt)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Topic>()
            .Property(c => c.UpdatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Topic>()
            .Property(c => c.CreatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<Topic>()
            .Property(c => c.DeletedAt)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<User>()
            .Property(c => c.UpdatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<User>()
            .Property(c => c.CreatedAt)
            .HasColumnType("timestamp without time zone");
        modelBuilder.Entity<User>()
            .Property(c => c.DeletedAt)
            .HasColumnType("timestamp without time zone");

        base.OnModelCreating(modelBuilder);
    }
}