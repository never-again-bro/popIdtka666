using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.Requests.Student;

public class CreateStudentRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string VkProfileLink { get; set; }
}