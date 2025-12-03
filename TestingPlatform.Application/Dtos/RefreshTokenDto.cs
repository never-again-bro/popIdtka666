using TestingPlatform.Application.Dtos;

public class RefreshTokenDto
{
    public UserDto User { get; set; }
    public DateTime Expires { get; set; }
}