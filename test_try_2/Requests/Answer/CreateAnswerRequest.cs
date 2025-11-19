using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Requests.Answer;

public class CreateAnswerRequest
{
    [Required]
    public int QuestionId { get; set; }
    [Required]
    public string Text { get; set; }
}