using TestingPlatform.Application.Dtos;
using TestingPlatform.Domain.Models;

namespace TestingPlatform.Application.Interfaces;

public interface ITestRepository
{
    Task<IEnumerable<object>> GetTopGroupsByTestCountAsync(int top = 10);
    Task<IEnumerable<object>> GetTestTimelineByPublicAsync();
    Task<IEnumerable<object>> GetDirectionAveragesAsync();
    Task<IEnumerable<object>> GetTestCountByTypeAsync();
    Task<IEnumerable<TestDto>> GetTopRecentAsync(int count);
    Task<IEnumerable<TestDto>> GetAllAsync(bool? isPublic, List<int> groupIds, List<int> studentIds);
    Task<TestDto> GetByIdAsync(int id);
    Task<int> CreateAsync(TestDto testDto);
    Task UpdateAsync(TestDto testDto);
    Task DeleteAsync(int id);
}