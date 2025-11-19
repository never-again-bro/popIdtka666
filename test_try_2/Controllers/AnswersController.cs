using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Infrastructure.Repositories;
using TestingPlatform.Requests.Answer;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswersController(IAnswerRepository answerRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAnswers()
    {
        var answers = await answerRepository.GetAllAsync();

        return Ok(mapper.Map<IEnumerable<AnswerResponse>>(answers));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAnswerById(int id)
    {
        var answer = await answerRepository.GetByIdAsync(id);

        return Ok(mapper.Map<AnswerResponse>(answer));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest answer)
    {
        var id = await answerRepository.CreateAsync(mapper.Map<AnswerDto>(answer));

        return StatusCode(StatusCodes.Status201Created, new { Id = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAnswer([FromBody] UpdateAnswerRequest answer)
    {
        await answerRepository.UpdateAsync(mapper.Map<AnswerDto>(answer));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        await answerRepository.DeleteAsync(id);

        return NoContent();
    }
}