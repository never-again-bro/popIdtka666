using homework.Models;
using System.ComponentModel.DataAnnotations;
using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public int Number { get; set; }
        [MaxLength(2000)]
        public string Description { get; set; }
        public AnswerType AnswerType { get; set; }
        public bool IsScoring { get; set; } = true;
        public int? MaxScore { get; set; }
        [Required]
        public int TestId { get; set; }
        public Test Test { get; set; }
        public List<UserAttemptAnswer> UserAttemptAnswers { get; set; }
        public List<Answer> Answers get; set; }
    }
}
