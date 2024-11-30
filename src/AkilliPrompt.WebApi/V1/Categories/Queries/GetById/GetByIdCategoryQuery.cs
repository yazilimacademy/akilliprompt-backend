using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Attributes;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetById;

[CacheOptions(absoluteExpirationMinutes: 960, slidingExpirationMinutes: 120)]
public sealed record GetByIdCategoryQuery : IRequest<GetByIdCategoryDto>, ICacheable
{
    public string CacheGroup => "Categories";

    [CacheKeyPart]
    public Guid Id { get; set; }

    public GetByIdCategoryQuery(Guid id)
    {
        Id = id;
    }
}
