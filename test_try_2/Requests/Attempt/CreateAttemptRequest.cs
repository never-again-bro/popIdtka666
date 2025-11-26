namespace TestingPlatform.Requests.Attempt;
public class CreateAttemptRequest
{
    public int TestId { get; set; }

    //TODO: удалить после добавления авторизации
    /// <summary>
    /// [Временно (после добавления авторизации будет удален)] Идентификатор студента
    /// </summary>
    public int StudentId { get; set; }
}