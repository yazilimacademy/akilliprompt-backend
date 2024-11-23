using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetById;

public sealed record GetByIdCategoryQuery(Guid Id) : IRequest<GetByIdCategoryDto>;
