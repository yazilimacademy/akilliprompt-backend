using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Create;

public sealed record CreatePromptCommand(
    string Title,
    string Description,
    string Content,
    IFormFile? Image,
    bool IsActive,
    List<Guid> CategoryIds,
    List<string>? PlaceholderNames = null
) : IRequest<ResponseDto<Guid>>;