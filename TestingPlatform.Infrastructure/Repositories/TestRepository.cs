using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Models;
using TestingPlatform.Infrastructure.Exceptions;

namespace TestingPlatform.Infrastructure.Repositories;

public class TestRepository(AppDbContext appDbContext, IMapper mapper) : ITestRepository
{
    public async Task<IEnumerable<object>> GetAllForStudentById(int studentId, int testId)
    {
        await RefreshPublicationStatusesAsync();
        var test = await appDbContext.Test
            .Where(t => t.IsPublic)
            .Where(t => t.Students.Any(s => s.Id == studentId))
            .Include(test => test.Directions)
            .Include(test => test.Courses)
            .Include(test => test.Groups)
            .Include(test => test.Projects)
            .Include(test => test.Students)
                .ThenInclude(student => student.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(test => test.Id == testId);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест не найден.");
        }

        return mapper.Map<IEnumerable<TestDto>>(test);
    }

    public async Task<IEnumerable<object>> GetTopGroupsByTestCountAsync(int top = 10)
    {
        return await appDbContext.Test
            .AsNoTracking()
            .SelectMany(t => t.Groups.Select(g => new
            {
                Group = g.Name,
                TestId = t.Id
            }))
            .GroupBy(x => x.Group)
            .Select(g => new
            {
                Group = g.Key,
                TestCount = g.Select(x => x.TestId).Distinct().Count()
            })
            .OrderByDescending(x => x.TestCount)
            .Take(top)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTestTimelineByPublicAsync()
    {
        return await appDbContext.Test
            .AsNoTracking()
            .Where(t => t.PublishedAt != default)                
            .GroupBy(t => new                                    
            {
                t.IsPublic,
                Year = t.PublishedAt.Year,
                Month = t.PublishedAt.Month
            })
            .Select(g => new                                    
            {
                g.Key.IsPublic,
                g.Key.Year,
                g.Key.Month,
                Count = g.Count()                                 
            })
            .OrderBy(x => x.Year)                                  
            .ThenBy(x => x.Month)
            .ThenByDescending(x => x.IsPublic)                     
            .ToListAsync();                                        
    }

    public async Task<IEnumerable<object>> GetDirectionAveragesAsync()
    {
        return await appDbContext.Directions
            .AsNoTracking()
            .Select(d => new
            {
                Direction = d.Name,
                AvgPassingScore = d.Tests.Average(t => (double?)t.PassingScore) ?? 0,
                AvgDuration = d.Tests.Average(t => (double?)t.DurationMinutes) ?? 0
            })
            .OrderByDescending(x => x.AvgPassingScore)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTestCountByTypeAsync()
    {
        return await appDbContext.Test
            .AsNoTracking()   
            .GroupBy(t => t.Type)          
            .Select(g => new              
            {
                Type = g.Key,              
                Count = g.Count()         
            })
            .ToListAsync();
    }

    private async Task RefreshPublicationStatusesAsync()
    {
        var now = DateTimeOffset.UtcNow;

        var publishCandidates = await appDbContext.Test
            .AsNoTracking()
            .Where(t => !t.IsPublic && (t.PublishedAt != null || t.Deadline != null))
            .Select(t => new { t.Id, t.PublishedAt, t.Deadline })
            .ToListAsync();

        var toPublishIds = publishCandidates
            .Where(x => x.PublishedAt != null
                        && x.PublishedAt <= now
                        && (x.Deadline == null || x.Deadline > now))
            .Select(x => x.Id)
            .ToList();

        if (toPublishIds.Count > 0)
            await appDbContext.Test
                .Where(t => toPublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, true));

        var unpublishCandidates = await appDbContext.Test
            .AsNoTracking()
            .Where(t => t.IsPublic && (t.PublishedAt == null || t.Deadline != null))
            .Select(t => new { t.Id, t.PublishedAt, t.Deadline })
            .ToListAsync();

        var toUnpublishIds = unpublishCandidates
            .Where(x => x.PublishedAt == null
                        || (x.Deadline != null && x.Deadline <= now))
            .Select(x => x.Id)
            .ToList();

        if (toUnpublishIds.Count > 0)
            await appDbContext.Test
                .Where(t => toUnpublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, false));
    }

    private async Task UpdateMembersTest(Test test, TestDto testDto)
    {
        var studentIds = testDto.Students?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var groupIds = testDto.Groups?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var courseIds = testDto.Courses?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var directionIds = testDto.Directions?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();
        var projectIds = testDto.Projects?.Select(x => x.Id).Where(id => id > 0).Distinct().ToArray() ?? Array.Empty<int>();

        if (appDbContext.Entry(test).State == EntityState.Detached)
            appDbContext.Attach(test);

        await appDbContext.Entry(test).Collection(t => t.Students).LoadAsync();
        test.Students.Clear();
        if (studentIds.Length > 0)
        {
            var students = await appDbContext.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();
            foreach (var s in students)
                test.Students.Add(s);
        }

        await appDbContext.Entry(test).Collection(t => t.Groups).LoadAsync();
        test.Groups.Clear();
        if (groupIds.Length > 0)
        {
            var groups = await appDbContext.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();
            foreach (var g in groups)
                test.Groups.Add(g);
        }

        await appDbContext.Entry(test).Collection(t => t.Courses).LoadAsync();
        test.Courses.Clear();
        if (courseIds.Length > 0)
        {
            var courses = await appDbContext.Courses
                .Where(c => courseIds.Contains(c.Id))
                .ToListAsync();
            foreach (var c in courses)
                test.Courses.Add(c);
        }

        await appDbContext.Entry(test).Collection(t => t.Directions).LoadAsync();
        test.Directions.Clear();
        if (directionIds.Length > 0)
        {
            var directions = await appDbContext.Directions
                .Where(d => directionIds.Contains(d.Id))
                .ToListAsync();
            foreach (var d in directions)
                test.Directions.Add(d);
        }

        await appDbContext.Entry(test).Collection(t => t.Projects).LoadAsync();
        test.Projects.Clear();
        if (projectIds.Length > 0)
        {
            var projects = await appDbContext.Projects
                .Where(p => projectIds.Contains(p.Id))
                .ToListAsync();
            foreach (var p in projects)
                test.Projects.Add(p);
        }
    }

    public async Task<IEnumerable<TestDto>> GetTopRecentAsync(int count = 5)
    {
        await RefreshPublicationStatusesAsync();
        var tests = await appDbContext.Test.AsNoTracking()
            .OrderByDescending(t => t.PublishedAt)
            .ThenByDescending(t => t.Id)
            .Take(count)
            .ToListAsync();

        return mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<IEnumerable<TestDto>> GetAllForStudent(int studentId)
    {
        await RefreshPublicationStatusesAsync();
        var tests = await appDbContext.Test
            .Where(t => t.IsPublic)

            .Where(t =>
                t.Students.Any(s => s.Id == studentId)

                || t.Courses.Any(c => c.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))

                || t.Projects.Any(p => p.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))

                || t.Directions.Any(d => d.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))
            )
            .ToListAsync();

        return mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<IEnumerable<TestDto>> GetAllAsync(bool? isPublic, List<int> groupIds, List<int> studentIds)
    {
        await RefreshPublicationStatusesAsync();
        var tests = appDbContext.Test
            .OrderByDescending(t => t.PublishedAt)
            .ThenBy(t => t.Title)
            .AsNoTracking()
            .AsQueryable();

        if (isPublic is not null)
            tests = tests.Where(t => t.IsPublic == isPublic);

        if (groupIds.Any())
            tests = tests.Where(t => t.Groups.Any(g => groupIds.Contains(g.Id)));

        if (studentIds.Any())
            tests = tests.Where(t => t.Students.Any(s => studentIds.Contains(s.Id)));

        return mapper.Map<IEnumerable<TestDto>>(await tests.ToListAsync());
    }

    public async Task<TestDto> GetByIdAsync(int id)
    {
        await RefreshPublicationStatusesAsync();
        var test = await appDbContext.Test
            .Include(test => test.Directions)
            .Include(test => test.Courses)
            .Include(test => test.Groups)
            .Include(test => test.Projects)
            .Include(test => test.Students)
                .ThenInclude(student => student.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест не найден.");
        }

        return mapper.Map<TestDto>(test);
    }

    public async Task<int> CreateAsync(TestDto testDto)
    {
        var test = mapper.Map<Test>(testDto);

        var testId = await appDbContext.AddAsync(test);

        await UpdateMembersTest(test, testDto);

        await appDbContext.SaveChangesAsync();

        return testId.Entity.Id;
    }

    public async Task UpdateAsync(TestDto testDto)
    {
        var test = await appDbContext.Test.FirstOrDefaultAsync(test => test.Id == testDto.Id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест не найден.");
        }

        test.Title = testDto.Title;
        test.Description = testDto.Description;
        test.IsRepeatable = testDto.IsRepeatable;
        test.Type = testDto.Type;
        test.PublishedAt = testDto.PublishedAt;
        test.Deadline = testDto.Deadline;
        test.DurationMinutes = testDto.DurationMinutes;
        test.IsPublic = testDto.IsPublic;
        test.PassingScore = testDto.PassingScore;
        test.MaxAttempts = testDto.MaxAttempts;

        await UpdateMembersTest(test, testDto);

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var test = await appDbContext.Test.FirstOrDefaultAsync(test => test.Id == id);

        if (test == null)
        {
            throw new EntityNotFoundException("Тест не найден.");
        }

        appDbContext.Test.Remove(test);

        await appDbContext.SaveChangesAsync();
    }
}


