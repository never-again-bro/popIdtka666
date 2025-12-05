using TestingPlatform.Responses.Student;
using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Responses.Auth;

public class AuthResponse
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public UserRole Role { get; set; }

    // добавили модель студента
    public StudentResponse? Student { get; set; }
}