using TestingPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Direction> Directions => Set<Direction>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Test> Test => Set<Test>();
    public DbSet<Question> Question => Set<Question>();
    public DbSet<Answer> Answer => Set<Answer>();
    public DbSet<Attempt> Attempt => Set<Attempt>();
    public DbSet<UserAttemptAnswer> UserAttemptAnswer => Set<UserAttemptAnswer>();
    public DbSet<UserSelectedOption> UserSelectedOption => Set<UserSelectedOption>();
    public DbSet<UserTextAnswer> UserTextAnswer => Set<UserTextAnswer>();
    public DbSet<TestResult> TestResult => Set<TestResult>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.TokenHash);

        modelBuilder.Entity<RefreshToken>()
           .HasOne(rt => rt.User)
           .WithMany(u => u.RefreshTokens)
           .HasForeignKey(rt => rt.UserId);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Login).IsUnique();
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Login).IsRequired();
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.PasswordHash).IsRequired();
            e.Property(x => x.Role).HasConversion<string>();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            e.HasOne(x => x.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Student>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Phone).HasMaxLength(30).IsRequired(); ;
            e.Property(x => x.VkProfileLink).IsRequired();
        });

        modelBuilder.Entity<Direction>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Group>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired();
            e.HasIndex(x => x.Name).IsUnique();

            e.HasOne(x => x.Direction)
                .WithMany(d => d.Groups)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Course)
                .WithMany(c => c.Groups)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Project)
                .WithMany(p => p.Groups)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Question>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TestId).IsRequired();
            e.Property(x => x.Number).IsRequired();
            e.Property(x => x.AnswerType).HasConversion<string>();
            e.HasOne(x => x.Test)
                .WithMany(s => s.Questions)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserAttemptAnswer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.AttemptId).IsRequired();
            e.Property(x => x.QuestionId).IsRequired();

            e.HasOne(x => x.Attempt)
               .WithMany(d => d.UserAttemptAnswers)
               .HasForeignKey(x => x.Id)
               .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Question)
               .WithMany(d => d.UserAttemptAnswers)
               .HasForeignKey(x => x.Id)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TestResult>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TestId).IsRequired();
            e.Property(x => x.StudentId).IsRequired();
            e.Property(x => x.AttemptId).IsRequired();
        });

        modelBuilder.Entity<Test>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Type).HasConversion<string>();
            e.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasMany(x => x.Groups)
               .WithMany(d => d.Tests)
               .UsingEntity(z => z.ToTable("test_groups"));
            e.HasMany(x => x.Students)
               .WithMany(d => d.Tests)
               .UsingEntity(z => z.ToTable("test_students"));
            e.HasMany(x => x.Projects)
               .WithMany(d => d.Tests)
               .UsingEntity(z => z.ToTable("test_projects"));
            e.HasMany(x => x.Courses)
               .WithMany(d => d.Tests)
               .UsingEntity(z => z.ToTable("test_courses"));
            e.HasMany(x => x.Directions)
               .WithMany(d => d.Tests)
               .UsingEntity(z => z.ToTable("test_directions"));
        });

        modelBuilder.Entity<Attempt>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            e.HasOne(x => x.Test)
               .WithMany(d => d.Attempts)
               .HasForeignKey(x => x.TestId)
               .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Student)
               .WithMany(d => d.Attempts)
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserTextAnswer>(e =>
        {
            e.HasOne(x => x.UserAttemptAnswer)
               .WithOne(d => d.UserTextAnswer)
               .HasForeignKey<UserTextAnswer>(x => x.UserAttemptAnswerId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserSelectedOption>(e =>
        {
            e.HasOne(x => x.UserAttemptAnswer)
               .WithMany(d => d.UserSelectedOptions)
               .HasForeignKey(x => x.Id)
               .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Answer)
               .WithMany(d => d.UserSelectedOptions)
               .HasForeignKey(x => x.Id)
               .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Answer>(e =>
        {
            e.HasOne(x => x.Question)
              .WithMany(d => d.Answers)
              .HasForeignKey(x => x.Id)
              .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TestResult>(e =>
        {
            e.HasOne(x => x.Test)
                .WithOne(d => d.TestResult)
                .HasForeignKey<Test>(x => x.Id)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Attempt)
                .WithOne(d => d.TestResult)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Student)
                .WithMany(d => d.TestResult)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
