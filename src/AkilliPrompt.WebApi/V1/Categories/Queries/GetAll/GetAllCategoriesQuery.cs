using MediatR;

namespace AkilliPrompt.WebApi.V1.Categories.Queries.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<List<GetAllCategoriesDto>>;
