using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Models;
using TestingPlatform.Infrastructure.Exceptions;

namespace TestingPlatform.Infrastructure.Repositories;

public class AttemptRepository(AppDbContext appDbContext, IMapper mapper, ITestRepository testRepository) : IAttemptRepository
{
    public async Task<int> CreateAsync(AttemptDto attemptDto)
    {
        var test = await testRepository.GetByIdAsync(attemptDto.TestId);

        if (test is null)
            throw new EntityNotFoundException("Тест не найден.");

        var student = await appDbContext.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == attemptDto.StudentId);

        if (student == null)
            throw new EntityNotFoundException("Студент не найден.");

        if (!test.IsPublic)
            throw new InvalidOperationException("Тест не доступен");

        // среди доступных ученику тестов нет, того, который он собирается проходить
        var availableTests = await testRepository.GetAllForStudent(attemptDto.StudentId);

        if (availableTests.All(t => t.Id != attemptDto.TestId))
            throw new InvalidOperationException("Доступ запрещен");

        var attempt = mapper.Map<Attempt>(attemptDto);

        if (test is { IsRepeatable: true, MaxAttempts: null })
            return await CreateAsync(attempt);

        var lastAttempts = await appDbContext.Attempt
            .Where(a => a.StudentId == attemptDto.StudentId && a.TestId == attemptDto.TestId)
            .ToListAsync();

        var inProgress = lastAttempts.FirstOrDefault(a => a.SubmittedAt == null);
        if (inProgress != null)
        {
            if (test.DurationMinutes.HasValue)
            {
                var expiresAt = inProgress.StartedAt.AddMinutes(test.DurationMinutes.Value);
                if (DateTimeOffset.UtcNow < expiresAt)
                {
                    throw new InvalidOperationException("Есть незавершённая попытка, время выполнения ещё не истекло.");
                }
            }
            else
            {
                throw new InvalidOperationException("Есть незавершённая попытка. Тест не имеет ограничений по времени, поэтому новую попытку начать нельзя.");
            }
        }

        if (!test.IsRepeatable && lastAttempts.Count > 0)
            throw new InvalidOperationException("Тест нельзя пройти более одного раза");

        if (test.IsRepeatable && lastAttempts.Count >= test.MaxAttempts)
            throw new InvalidOperationException("Исчерпано количество прохождений теста");

        return await CreateAsync(attempt);
    }

    private async Task<int> CreateAsync(Attempt attempt)
    {
        attempt.StartedAt = DateTime.Now;
        attempt.Score = 0;
        var attemptId = await appDbContext.AddAsync(attempt);
        await appDbContext.SaveChangesAsync();

        return attemptId.Entity.Id;
    }

    public async Task UpdateAsync(AttemptDto attemptDto)
    {
        var attempt = await appDbContext.Attempt
            .Include(attempt => attempt.UserAttemptAnswers)
            .FirstOrDefaultAsync(a => a.Id == attemptDto.Id);

        if (attempt is null)
            throw new EntityNotFoundException("Попытка не найдена");

        if (attempt.SubmittedAt != null)
            throw new InvalidOperationException("Нельзя завершить уже сданную попытку.");

        attempt.SubmittedAt = DateTime.Now;

        var score = attempt.UserAttemptAnswers.Sum(ua => ua.ScoreAwarded);
        attempt.Score = score;

        var test = await appDbContext.Test.AsNoTracking().FirstOrDefaultAsync(test => test.Id == attempt.TestId);

        var testResult = await appDbContext.TestResult
            .Include(tr => tr.Attempt)
            .FirstOrDefaultAsync(tr => tr.TestId == attempt.TestId);

        if (testResult == null)
        {
            var newtestResult = new TestResult
            {
                AttemptId = attempt.Id,
                StudentId = attempt.StudentId,
                TestId = attempt.TestId,
                Passed = test!.PassingScore == null || test.PassingScore <= attempt.Score,
            };

            await appDbContext.TestResult.AddAsync(newtestResult);
        }
        else
        {
            if (testResult.Attempt.Score < attempt.Score)
            {
                testResult.AttemptId = attempt.Id;
                testResult.Passed = test!.PassingScore == null || test.PassingScore <= attempt.Score;
            }
        }

        await appDbContext.SaveChangesAsync();
    }
}