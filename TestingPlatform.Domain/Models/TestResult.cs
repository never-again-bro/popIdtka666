using homework.Models;
using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Domain.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        [Required]
        public bool Passed { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int AttemptId { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Test Test { get; set; }
        public List<Attempt> Attempt { get; set; }
        public Student Student { get; set; }
    }
}