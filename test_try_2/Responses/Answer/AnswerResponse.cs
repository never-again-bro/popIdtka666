using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Requests.Answer;

public class AnswerResponse
{
    public int QuestionId { get; set; }
    public string Text { get; set; }
}