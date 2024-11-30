using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Attributes;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetAll;

[CacheOptions(absoluteExpirationMinutes: 960, slidingExpirationMinutes: 120)]
public sealed record GetAllCategoriesQuery : IRequest<List<GetAllCategoriesDto>>, ICacheable
{
    public string CacheGroup => "Categories";
}
