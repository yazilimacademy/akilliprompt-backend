using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Attributes;
using AkilliPrompt.WebApi.Interfaces;
using AkilliPrompt.WebApi.Models;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Prompts.Queries.GetAll;


[CacheOptions(absoluteExpirationMinutes: 600, slidingExpirationMinutes: 120)]
public sealed record GetAllPromptsQuery : IRequest<PaginatedList<GetAllPromptsDto>>, ICacheable, IPaginated
{
    public string CacheGroup => "Prompts";

    [CacheKeyPart]
    public string? SearchKeyword { get; set; } = null;
    [CacheKeyPart]
    public List<Guid> CategoryIds { get; set; } = [];
    [CacheKeyPart]
    public int PageNumber { get; }
    [CacheKeyPart]
    public int PageSize { get; }

    public GetAllPromptsQuery(int pageNumber, int pageSize, string? searchKeyword, List<Guid> categoryIds)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchKeyword = searchKeyword;
        CategoryIds = categoryIds;
    }
}

