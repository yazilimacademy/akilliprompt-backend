using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Commands.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<ResponseDto<Guid>>;
