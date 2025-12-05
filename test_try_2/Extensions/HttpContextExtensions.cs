using TestingPlatform.Constants;
using System.Security.Authentication;

namespace TestingPlatform.Extensions;

public static class HttpContextExtensions
{
    public static int TryGetUserId(this HttpContext httpContext)
    {
        var studentIdValue = httpContext.User.Claims.FirstOrDefault(c => c.Type == TestingPlatformClaimTypes.StudentId)?.Value;

        if (!int.TryParse(studentIdValue, out var studentId))
        {
            throw new AuthenticationException("Данные о пользователе пусты.");
        }

        return studentId;
    }
}