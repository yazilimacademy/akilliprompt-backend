using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Commands.Delete;

public sealed record DeletePromptCommand(Guid Id) : IRequest<ResponseDto<Guid>>;
