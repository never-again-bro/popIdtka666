using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Domain.Models
{
    public class UserTextAnswer
    {
        public int Id { get; set; }
        [Required]
        public string TextAnswer { get; set; }
        [Required]
        public string UserAttemptAnswerId { get; set; }
        public UserAttemptAnswer UserAttemptAnswer { get; set; }
    }
}
