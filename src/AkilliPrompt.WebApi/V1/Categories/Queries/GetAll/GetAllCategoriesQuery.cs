using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Helpers;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<List<GetAllCategoriesDto>>, ICacheable
{
    public string CacheKey => CacheKeysHelper.GetAllCategoriesKey;
}
