using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Requests.Auth;
using TestingPlatform.Responses.Auth;
using TestingPlatform.Services;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;

namespace TestingPlatform.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class AuthController(IAuthRepository authRepository, ITokenService tokenService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] AuthRequest auth)
    {
        var userLoginDto = mapper.Map<UserLoginDto>(auth);
        var user = await authRepository.AuthorizeUser(userLoginDto);
        var response = mapper.Map<AuthResponse>(user);

        var accessToken = tokenService.CreateAccessToken(response);

        return Ok(accessToken);
    }
}