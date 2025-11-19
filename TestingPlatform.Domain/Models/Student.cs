using System.ComponentModel.DataAnnotations;
namespace TestingPlatform.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required] [MaxLength(30)]
        public string VkProfileLink { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Attempt> Attempts { get; set; }
        public List<TestResult> TestResult { get; set; }
        public List<Test> Tests { get; set; }
    }
}
