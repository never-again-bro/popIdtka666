//using Microsoft.AspNetCore.Mvc;

//namespace test_try_2.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class AnswersController : ControllerBase
//{
//    [HttpGet]
//    public IActionResult GetAllAnswers() => Ok("Список ответов");

//    [HttpGet("{id}")]
//    public IActionResult GetAnswerById(int id)
//    {
//        if (id == 1) return Ok("Ответ 1");
//        return NotFound();
//    }

//    [HttpPost]
//    public IActionResult CreateAnswer() => Created("/api/tests/1", "Создан ответ с ID=1");

//    [HttpPut("{id}")]
//    public IActionResult UpdateAnswer(int id) => NoContent();

//    [HttpDelete("{id}")]
//    public IActionResult DeleteAnswer(int id) => NoContent();
//}