using AkilliPrompt.Domain.Common;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetById;

public sealed record GetPromptByIdQuery(Guid Id) : IRequest<GetPromptByIdDto>, ICacheable
{
    public string CacheGroup => "Prompts";
}
