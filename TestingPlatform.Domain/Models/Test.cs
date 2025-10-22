using System.ComponentModel.DataAnnotations;
using TestingPlatform.Domain.Enums;
using TestingPlatform.Domain.Models;

namespace homework.Models;
public class Test
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public bool IsRepeatable { get; set; } = false;
    public TestType Type { get; set; }
    public AnswerType AnswerType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    [Required]
    public DateTimeOffset PublishedAt { get; set; }
    [Required]
    public DateTimeOffset Deadline { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; } = false;
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }
    public List<Question> Questions { get; set; }
    public List<Student> Students { get; set; }
    public List<Project> Projects { get; set; }
    public List<Course> Courses { get; set; }
    public List<Group> Groups { get; set; }
    public List<Direction> Directions { get; set; }
    public List<Attempt> Attempts { get; set; }
    public TestResult TestResult { get; set; }
}


