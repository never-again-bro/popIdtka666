using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Responses.Student;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Domain.Enums;
using TestingPlatform.Requests.Student;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentRepository studentRepository, IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await studentRepository.GetAllAsync();

        return Ok(mapper.Map<IEnumerable<StudentResponse>>(students));
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await studentRepository.GetByIdAsync(id);

        return Ok(mapper.Map<StudentResponse>(student));
    }
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest student)
    {
        var userDto = new UserDto()
        {
            Login = student.Login,
            Password = student.Password,
            Email = student.Email,
            FirstName = student.FirstName,
            MiddleName = student.MiddleName,
            LastName = student.LastName,
            Role = UserRole.Student
        };
        var userId = await userRepository.CreateAsync(userDto);

        var studentDto = new StudentDto()
        {
            UserId = userId,
            Phone = student.Phone,
            VkProfileLink = student.VkProfileLink
        };

        var studentId = await studentRepository.CreateAsync(studentDto);

        return StatusCode(StatusCodes.Status201Created, new { Id = studentId });
    }
    [HttpPut]
    public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentRequest student)
    {
        await studentRepository.UpdateAsync(mapper.Map<StudentDto>(student));

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        await studentRepository.DeleteAsync(id);

        return NoContent();
    }
}