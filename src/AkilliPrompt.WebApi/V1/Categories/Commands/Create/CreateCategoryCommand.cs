using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name, string Description) : IRequest<ResponseDto<Guid>>;
