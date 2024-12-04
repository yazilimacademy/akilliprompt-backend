using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Attributes;
using AkilliPrompt.WebApi.Interfaces;
using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.PromptComments.Queries.GetAll;

[CacheOptions(absoluteExpirationMinutes: 600, slidingExpirationMinutes: 120)]
public sealed record GetAllPromptCommentsQuery : IRequest<PaginatedList<GetAllPromptCommentsDto>>, ICacheable, IPaginated
{
    public string CacheGroup => "PromptComments";

    [CacheKeyPart]
    public Guid PromptId { get; set; }
    [CacheKeyPart]
    public int PageNumber { get; set; }
    [CacheKeyPart]
    public int PageSize { get; set; }

    public GetAllPromptCommentsQuery(Guid promptId, int pageNumber = 1, int pageSize = 10)
    {
        PromptId = promptId;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

