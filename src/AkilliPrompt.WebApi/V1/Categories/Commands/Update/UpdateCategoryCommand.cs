using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest<ResponseDto<Guid>>;
