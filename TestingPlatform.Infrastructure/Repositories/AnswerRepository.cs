using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Models;
using TestingPlatform.Infrastructure.Exceptions;

namespace TestingPlatform.Infrastructure.Repositories;

public class AnswerRepository(AppDbContext appDbContext, IMapper mapper) : IAnswerRepository
{
    public async Task<List<AnswerDto>> GetAllAsync()
    {
        var answers = await appDbContext.Answer
            .Include(g => g.Question)
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<List<AnswerDto>>(answers);
    }

    public async Task<AnswerDto> GetByIdAsync(int id)
    {
        var answer = await appDbContext.Answer
            .Include(g => g.Question)
            .AsNoTracking()
            .FirstOrDefaultAsync(answer => answer.Id == id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ не найден.");
        }

        return mapper.Map<AnswerDto>(answer);
    }


    public async Task<int> CreateAsync(AnswerDto answerDto)
    {
        var answer = mapper.Map<Answer>(answerDto);

        var question = await appDbContext.Question.FirstOrDefaultAsync(d => d.Id == answerDto.Question.Id);

        if (question is null)
        {
            throw new EntityNotFoundException("Вопрос не найден.");
        }

        answer.QuestionId = question.Id;


        var answerId = await appDbContext.AddAsync(answer);
        await appDbContext.SaveChangesAsync();

        return answerId.Entity.Id;
    }

    public async Task UpdateAsync(AnswerDto answerDto)
    {
        var answer = await appDbContext.Answer.FirstOrDefaultAsync(group => group.Id == answerDto.Id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ не найдена.");
        }

        answer.Text = answerDto.Text;

        await appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var answer = await appDbContext.Answer.FirstOrDefaultAsync(answer => answer.Id == id);

        if (answer == null)
        {
            throw new EntityNotFoundException("Ответ не найдена.");
        }

        appDbContext.Answer.Remove(answer);
        await appDbContext.SaveChangesAsync();
    }
}