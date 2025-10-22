using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Domain.Models
{
    public class UserSelectedOption
    {  
        public int Id { get; set; }
        [Required]
        public int UserAttemptAnswerId { get; set; }
        [Required]
        public int AnswerId { get; set; }
        public UserAttemptAnswer UserAttemptAnswer { get; set; }
        public Answer Answer { get; set; }
    }
}