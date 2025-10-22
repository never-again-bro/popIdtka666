using homework.Models;
using System.ComponentModel.DataAnnotations;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.Domain.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public int? Score { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Test Test { get; set; }
        public Student Student { get; set; }
        public List<UserAttemptAnswer> UserAttemptAnswers { get; set; }
        public TestResult TestResult { get; set; }
    }
}
