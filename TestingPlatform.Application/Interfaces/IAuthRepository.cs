using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IAuthRepository
{
    Task<UserDto> AuthorizeUser(UserLoginDto userLoginDto);
}