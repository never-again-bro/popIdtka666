using System.ComponentModel.DataAnnotations;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Requests.Test;

public class UpdateTestRequest
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public TestType Type { get; set; }
    public AnswerType AnswerType { get; set; }
    [Required]
    public int? DurationMinutes { get; set; }
    [Required]
    public int? PassingScore { get; set; }
    [Required]
    public int? MaxAttempts { get; set; }
    [Required]
    public bool IsRepeatable { get; set; } = false;
    [Required]
    public DateTimeOffset Deadline { get; set; }
    [Required]
    public DateTime PublishedAt { get; set; }
    public List<StudentDto> Students { get; set; }
    public List<ProjectDto> Projects { get; set; }
    public List<CourseDto> Courses { get; set; }
    public List<GroupDto> Groups { get; set; }
    public List<DirectionDto> Directions { get; set; }
}