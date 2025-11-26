using TestingPlatform.Application.Dtos;

namespace TestingPlatform.Application.Interfaces;

public interface IAttemptRepository
{
    Task<int> CreateAsync(AttemptDto attemptDto);
    Task UpdateAsync(AttemptDto attemptDto);
}