using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Helpers;
using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetById;

public sealed record GetByIdCategoryQuery(Guid Id) : IRequest<GetByIdCategoryDto>, ICacheable
{
    public string CacheKey => CacheKeysHelper.GetByIdCategoryKey(Id);
}
