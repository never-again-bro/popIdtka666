using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Requests.Student;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

[ApiController]
[Route("api/[controller]")]
public class TestsController(ITestRepository testRepository, IMapper mapper) : ControllerBase
{
    [HttpGet("manage")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(IEnumerable<TestResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTestsForManager([FromQuery] bool? isPublic, [FromQuery] List<int> groupIds, [FromQuery] List<int> studentIds)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var tests = await testRepository.GetAllAsync(isPublic, groupIds, studentIds);

        return Ok(mapper.Map<IEnumerable<TestResponse>>(tests));
    }

    [HttpGet]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetTestsForStudent([FromQuery] bool? isPublic, [FromQuery] List<int> groupIds, [FromQuery] List<int> studentIds)
    {
        var tests = await testRepository.GetAllAsync(isPublic, groupIds, studentIds); 

        return Ok(mapper.Map<IEnumerable<TestResponse>>(tests));
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTestById(int id)
    {
        var test = await testRepository.GetByIdAsync(id);

        return Ok(mapper.Map<TestResponse>(test));
    }
    [HttpPost]
    public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest test)
    {
        var testDto = new TestDto()
        {
            Title = test.Title,
            Description = test.Description,
            IsRepeatable = test.IsRepeatable,
            DurationMinutes = test.DurationMinutes,
            PassingScore = test.PassingScore,
            MaxAttempts = test.MaxAttempts,
            Deadline = test.Deadline
        };

        var testId = await testRepository.CreateAsync(testDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = testId });
    }
    [HttpPut]
    public async Task<IActionResult> UpdateTest([FromBody] UpdateTestRequest test)
    {
        await testRepository.UpdateAsync(mapper.Map<TestDto>(test));

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        await testRepository.DeleteAsync(id);

        return NoContent();
    }
} 



