using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using practice.Requests.Question;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Enums;
using TestingPlatform.Requests.Question;
using TestingPlatform.Responses.Question;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController(IQuestionRepository questionRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetQuestions()
    {
        var questions = await questionRepository.GetAllAsync();

        return Ok(mapper.Map<IEnumerable<QuestionResponse>>(questions));
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuestionById(int id)
    {
        var question = await questionRepository.GetByIdAsync(id);

        return Ok(mapper.Map<QuestionResponse>(question));
    }
    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest question)
    {
        var questionDto = new QuestionDto()
        {
            Text = question.Text,
            Number = question.Number,
            Description = question.Description,
            AnswerType = question.AnswerType,
            IsScoring = question.IsScoring,
            MaxScore = question.MaxScore,
            TestId = question.TestId
        };

        var questionId = await questionRepository.CreateAsync(questionDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = questionId });
    }
    [HttpPut]
    public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest question)
    {
        await questionRepository.UpdateAsync(mapper.Map<QuestionDto>(question));

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        await questionRepository.DeleteAsync(id);

        return NoContent();
    }
}