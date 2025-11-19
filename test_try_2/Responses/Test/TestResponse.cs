using System.ComponentModel.DataAnnotations;
using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Responses.Test;

public class TestResponse
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TestType Type { get; set; }
    public AnswerType AnswerType { get; set; }
    public int? DurationMinutes { get; set; }
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }
    public bool IsRepeatable { get; set; } = false;
    public DateTimeOffset Deadline { get; set; }
}