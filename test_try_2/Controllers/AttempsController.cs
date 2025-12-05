using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Extensions;
using TestingPlatform.Requests.Attempt;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Authorize(Roles = "Student")]
public class AttemptsController(IAttemptRepository attemptRepository, IMapper mapper) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAttempt(CreateAttemptRequest attempt)
    {
        var studentId = HttpContext.TryGetUserId();

        var attemptDto = mapper.Map<AttemptDto>(attempt);
        attemptDto.StudentId = studentId;

        var attemptId = await attemptRepository.CreateAsync(attemptDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = attemptId });
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAttempt(UpdateAttemptRequest attempt)
    {
        var studentId = HttpContext.TryGetUserId();

        var attemptDto = mapper.Map<AttemptDto>(attempt);
        attemptDto.StudentId = studentId;

        await attemptRepository.UpdateAsync(attemptDto);

        return StatusCode(StatusCodes.Status200OK);
    }
}