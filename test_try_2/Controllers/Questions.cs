//using Microsoft.AspNetCore.Mvc;

//namespace test_try_2.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class QuestionsController : ControllerBase
//{
//    [HttpGet]
//    public IActionResult GetAllQuestions() => Ok("Список вопросов");

//    [HttpGet("{id}")]
//    public IActionResult GetQuestionById(int id)
//    {
//        if (id == 1) return Ok("Вопрос 1");
//        return NotFound();
//    }

//    [HttpPost]
//    public IActionResult CreateQuestion() => Created("/api/tests/1", "Создан вопрос с ID=1");

//    [HttpPut("{id}")]
//    public IActionResult UpdateQuestion(int id) => NoContent();

//    [HttpDelete("{id}")]
//    public IActionResult DeleteQuestion(int id) => NoContent();
//}