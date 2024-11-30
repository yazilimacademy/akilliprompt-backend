using AkilliPrompt.WebApi.V1.Auth.Commands.GoogleLogin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AkilliPrompt.WebApi.V1.Auth;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLoginAsync(GoogleLoginCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}
