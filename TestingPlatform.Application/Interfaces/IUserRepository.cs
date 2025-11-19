using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task<int> CreateAsync(UserDto userDto);
    Task UpdateAsync(UserDto userDto);
    Task DeleteAsync(int id);
}