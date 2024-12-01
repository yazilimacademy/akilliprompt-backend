using AkilliPrompt.WebApi.V1.Prompts.Commands.Create;
using AkilliPrompt.WebApi.V1.Prompts.Commands.Delete;
using AkilliPrompt.WebApi.V1.Prompts.Queries.GetAll;
using AkilliPrompt.WebApi.V1.Prompts.Queries.GetById;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AkilliPrompt.WebApi.V1.Prompts;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public sealed class PromptsController : ControllerBase
{

    private readonly ISender _mediator;

    public PromptsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("get-all")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAllAsync(GetAllPromptsQuery query, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetPromptByIdQuery(id), cancellationToken));
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAsync([FromForm] CreatePromptCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new DeletePromptCommand(id), cancellationToken));
    }
}